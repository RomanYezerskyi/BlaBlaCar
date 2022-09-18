using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.DTOs;
using BlaBlaCar.BL.DTOs.FeedbackDTOs;
using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Exceptions;
using BlaBlaCar.BL.Hubs;
using BlaBlaCar.BL.Hubs.Interfaces;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Entities.NotificationEntities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlaBlaCar.BL.Services.NotificationServices
{
    public class NotificationService: INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HostSettings _hostSettings;
        private readonly IHubContext<NotificationHub, INotificationsHubClient> _hubContext;
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<HostSettings> hostSettings, IHubContext<NotificationHub, INotificationsHubClient> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
            _hostSettings = hostSettings.Value;
        }
        public async Task<IEnumerable<GetNotificationsDTO>> GetUserUnreadNotificationsAsync(ClaimsPrincipal principal)
        {
            var userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var readNotifi = await _unitOfWork.ReadNotifications
                .GetAsync(null,null,x => x.UserId == userId);

            Expression<Func<Notifications, bool>> notificationFilter =
                x => x.UserId == userId || x.NotificationStatus == NotificationStatus.Global;
            if (readNotifi.Any())
            {
                notificationFilter =
                    x => x.UserId == userId || x.NotificationStatus == NotificationStatus.Global
                        && readNotifi.ToList().Any(r => r.NotificationId != x.Id);
            }
            var notifications = _mapper.Map<IEnumerable<GetNotificationsDTO>>(
                await _unitOfWork.Notifications.GetAsync(
                    x=>x.OrderByDescending(x=>x.CreatedAt),
                    null, notificationFilter));

            if (!notifications.Any())
                throw new NotFoundException(nameof(NotificationsDTO));

            var readNotifications = _mapper.Map<IEnumerable<ReadNotificationsDTO>>(
                await _unitOfWork.ReadNotifications.GetAsync(null, null, x => x.UserId == userId));

            notifications = notifications.Select(n =>
            {
                if (readNotifications.Any(x => x.NotificationId == n.Id))
                    n.ReadNotificationStatus = ReadNotificationStatusDTO.Read;
                return n;
            });
            return notifications;
        }
        public async Task<IEnumerable<GetNotificationsDTO>> GetUserNotificationsAsync(ClaimsPrincipal principal, int take, int skip)
        {
            var userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);

            var readNotifi = await _unitOfWork.ReadNotifications
                .GetAsync(null, x=>x.Include(x=>x.Notification),
                    x => x.UserId == userId,
                    take: take == 0 ? int.MaxValue : take,
                    skip: skip);
            var notifications = _mapper.Map<IEnumerable<GetNotificationsDTO>>( 
                readNotifi.Select(x => x.Notification));

            //var notifications = _mapper.Map<IEnumerable<GetNotificationsDTO>>(
            //    await _unitOfWork.Notifications.GetAsync(
            //        x => x.OrderByDescending(x => x.CreatedAt),
            //        null, x => x.UserId == userId || x.NotificationStatus == NotificationStatus.Global,
            //        take: take == 0 ? int.MaxValue : take,
            //        skip: skip));
            if (!notifications.Any())
                throw new NotFoundException(nameof(NotificationsDTO));

            var readNotifications = _mapper.Map<IEnumerable<ReadNotificationsDTO>>(
                await _unitOfWork.ReadNotifications.GetAsync(null, null, x => x.UserId == userId));

            notifications = notifications.Select(n =>
            {
                if (readNotifications.Any(x => x.NotificationId == n.Id))
                    n.ReadNotificationStatus = ReadNotificationStatusDTO.Read;
                return n;
            });
            return notifications;
        }
        public async Task<bool> CreateNotificationAsync(CreateNotificationDTO notificationModel, ClaimsPrincipal principal)
        {
            var notification = _mapper.Map<Notifications>(notificationModel);

            await _unitOfWork.Notifications.InsertAsync(notification);
            var createdBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var result = await _unitOfWork.SaveAsync(createdBy);
            if (notificationModel.NotificationStatus == NotificationStatusDTO.SpecificUser && result)
            {
                await _hubContext.Clients.Group(notificationModel.UserId.ToString()).BroadcastNotification();
            }
            else
            {
                await _hubContext.Clients.All.BroadcastNotification();
            }

            return result;
            
        }

        public async Task GenerateNotificationAsync(CreateNotificationDTO notificationModel)
        {
            var notification = _mapper.Map<Notifications>(notificationModel);
            await _hubContext.Clients.Group(notificationModel.UserId.ToString()).BroadcastNotification();
            await _unitOfWork.Notifications.InsertAsync(notification);
        }
        public async Task GenerateFeedBackNotificationAsync(Guid tripId)
        {
            var trip = await _unitOfWork.Trips
                .GetAsync( x=>x.Include(x=>x.TripUsers)
                    .Include(x=>x.User), x => x.Id == tripId);
            if (trip == null) throw new NotFoundException($"Trips with id {tripId}");
            var users = trip.TripUsers.Distinct();

            foreach (var user in users)
            {
                var notificationDTO = new NotificationsDTO()
                {
                    UserId = user.UserId,
                    NotificationStatus = NotificationStatusDTO.FeedBack,
                    Text = $"Please write a feedback about the driver {trip.User.FirstName}." +
                           $"\nDescribe how your trip {trip.StartPlace} - {trip.EndPlace} went.",
                    FeedBackOnUser = trip.UserId,
                };
                await _unitOfWork.Notifications.InsertAsync(_mapper.Map<Notifications>(notificationDTO));
                await _unitOfWork.SaveAsync(trip.UserId);
                await _hubContext.Clients.Group(user.Id.ToString()).BroadcastNotification();
            }
        }

        public async Task AddFeedBack(CreateFeedbackDTO newFeedback, ClaimsPrincipal principal)
        {
            Guid userId = Guid.Parse(principal.Claims.FirstOrDefault(x=>x.Type == JwtClaimTypes.Id).Value);
            var currentUser = await _unitOfWork.Users.GetAsync(null, x => x.Id == userId);
            if (currentUser == null) throw new NotFoundException("User not found!");

            var feedbackDTO = _mapper.Map<FeedBackDTO>(newFeedback);
            await _unitOfWork.FeedBacks.InsertAsync(_mapper.Map<FeedBack>(feedbackDTO));
            var notification = new CreateNotificationDTO()
            {
                UserId = feedbackDTO.UserId,
                NotificationStatus = NotificationStatusDTO.FeedBack,
                Text = $"{currentUser.FirstName} added feedback on your trip !"
            };
            await GenerateNotificationAsync(notification);
            await _unitOfWork.SaveAsync(currentUser.Id);
        }

        public async Task<bool> ReadAllNotificationAsync(IEnumerable<NotificationsDTO> notification, ClaimsPrincipal principal)
        {
            var readNotifications = new List<ReadNotificationsDTO>();

            readNotifications.AddRange(
                notification.Select(n => new ReadNotificationsDTO() { NotificationId = n.Id, UserId = n.UserId }));

            await _unitOfWork.ReadNotifications.InsertRangeAsync(_mapper.Map<IEnumerable<ReadNotifications>>(readNotifications));

            var createdBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(createdBy);
        }
        public async Task<IEnumerable<GetNotificationsDTO>> GetGlobalNotificationsAsync(int take,int skip)
        {
            var notifications = _mapper.Map<IEnumerable<GetNotificationsDTO>>(
                await _unitOfWork.Notifications.GetAsync(
                    x => x.OrderByDescending(x => x.CreatedAt),
                    null, x => x.NotificationStatus == NotificationStatus.Global, 
                    take:take,
                    skip:skip));

            return notifications;
        }
        public async Task<IEnumerable<GetNotificationsDTO>> GetUsersNotificationsAsync(int take, int skip)
        {
            var notifications = _mapper.Map<IEnumerable<GetNotificationsDTO>>(
                await _unitOfWork.Notifications.GetAsync(
                    x => x.OrderByDescending(x => x.CreatedAt),
                    x=>x.Include(x=>x.User), 
                    x => x.NotificationStatus == NotificationStatus.SpecificUser,
                    take: take,
                    skip: skip));

            notifications = notifications.Select(n =>
            {
                if (n.User.UserImg != null)
                {
                    n.User.UserImg = _hostSettings.CurrentHost + n.User.UserImg;
                }
                return n;
            });

            return notifications;
        }
    }

    
}
