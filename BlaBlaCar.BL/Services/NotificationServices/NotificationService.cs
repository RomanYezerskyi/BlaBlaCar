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
using BlaBlaCar.BL.Services.MapsServices;
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
        private readonly IMapService _mapService;
        public NotificationService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IOptionsSnapshot<HostSettings> hostSettings, 
            IHubContext<NotificationHub, INotificationsHubClient> hubContext, 
            IMapService mapService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
            _mapService = mapService;
            _hostSettings = hostSettings.Value;
        }
        public async Task<IEnumerable<GetNotificationsDTO>> GetUserUnreadNotificationsAsync(Guid currentUserId)
        {
            var readNotifications = _mapper.Map<IEnumerable<ReadNotificationsDTO>>(
                await _unitOfWork.ReadNotifications.GetAsync(null, null, x => x.UserId == currentUserId));

            Expression<Func<Notifications, bool>> notificationFilter =
                x => x.UserId == currentUserId || x.NotificationStatus == NotificationStatus.Global;
            IEnumerable<GetNotificationsDTO?> notifications = _mapper.Map<IEnumerable<GetNotificationsDTO>>(
                await _unitOfWork.Notifications.GetAsync(
                    x=>x.OrderByDescending(x=>x.CreatedAt),
                    null, notificationFilter));
            if (!notifications.Any()) return null;
            var result = notifications.Where(n =>
                readNotifications.All(x => x.NotificationId != n.Id));
            return result;
        }
        public async Task<IEnumerable<GetNotificationsDTO>> GetUserNotificationsAsync(int take, int skip, Guid currentUserId)
        {
            var readNotifications = _mapper.Map<IEnumerable<ReadNotificationsDTO>>( await _unitOfWork.ReadNotifications
                .GetAsync(null, x=>x.Include(x=>x.Notification),
                    x => x.UserId == currentUserId,
                    take: take == 0 ? int.MaxValue : take,
                    skip: skip));
            var notifications = _mapper.Map<IEnumerable<GetNotificationsDTO>>( 
                readNotifications.Select(x => x.Notification));

            if (!notifications.Any()) return null;
                notifications = notifications.Select(n =>
            {
                if (readNotifications.Any(x => x.NotificationId == n.Id))
                    n.ReadNotificationStatus = ReadNotificationStatusDTO.Read;
                return n;
            });
            return notifications;
        }
        public async Task<bool> CreateNotificationAsync(CreateNotificationDTO notificationModel, Guid currentUserId)
        {
            var notification = _mapper.Map<Notifications>(notificationModel);

            await _unitOfWork.Notifications.InsertAsync(notification);
            var result = await _unitOfWork.SaveAsync(currentUserId);
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
            var users = trip.TripUsers.ToList().GroupBy(x => x.UserId).Select(x => x.FirstOrDefault());

            var startPlace = await _mapService.GetPlaceInformation(trip.StartLocation.X, trip.StartLocation.Y);
            var endPlace = await _mapService.GetPlaceInformation(trip.EndLocation.X, trip.EndLocation.Y);


            foreach (var user in users)
            {
                var notificationDTO = new NotificationsDTO()
                {
                    UserId = user.UserId,
                    NotificationStatus = NotificationStatusDTO.RequestForFeedBack,
                    Text = $"Please write a feedback about the driver {trip.User.FirstName}." +
                           $"\nDescribe how your trip {startPlace?.FeaturesList.First().Properties.City} - {endPlace?.FeaturesList.First().Properties.City} went.",
                    FeedBackOnUser = trip.UserId,
                };
                await _unitOfWork.Notifications.InsertAsync(_mapper.Map<Notifications>(notificationDTO));
                await _unitOfWork.SaveAsync(trip.UserId);
                await _hubContext.Clients.Group(user.UserId.ToString()).BroadcastNotification();
            }
        }

        public async Task AddFeedBack(CreateFeedbackDTO newFeedback, Guid currentUserId)
        {
            var currentUser = await _unitOfWork.Users.GetAsync(null, x => x.Id == currentUserId);
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

        public async Task<IEnumerable<FeedBackDTO>> GetUserFeedBacks(Guid currentUserId, int take, int skip)
        {
            var feedBacks = _mapper.Map<IEnumerable<FeedBackDTO>>(
                await _unitOfWork.FeedBacks.GetAsync(
                    includes: null,
                    orderBy: x=>x.OrderByDescending(x=>x.CreatedAt),
                    filter: x => x.UserId == currentUserId,
                    take: take,
                    skip: skip
                )
            );
            return feedBacks;
        }


        public async Task<bool> ReadAllNotificationAsync(IEnumerable<NotificationsDTO> notification, Guid currentUserId)
        {
            var readNotifications = new List<ReadNotificationsDTO>();

            readNotifications.AddRange(
                notification.Select(n => new ReadNotificationsDTO() { NotificationId = n.Id, UserId = currentUserId }));

            await _unitOfWork.ReadNotifications.InsertRangeAsync(_mapper.Map<IEnumerable<ReadNotifications>>(readNotifications));

            return await _unitOfWork.SaveAsync(currentUserId);
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
