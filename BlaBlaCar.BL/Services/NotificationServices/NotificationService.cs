using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Exceptions;
using BlaBlaCar.BL.Hubs;
using BlaBlaCar.BL.Interfaces;
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
        private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<HostSettings> hostSettings, IHubContext<BroadcastHub, IHubClient> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
            _hostSettings = hostSettings.Value;
        }
        public async Task<IEnumerable<GetNotificationsDTO>> GetUserNotificationsAsync(ClaimsPrincipal principal)
        {
            var userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var notifications = _mapper.Map<IEnumerable<GetNotificationsDTO>>(
                await _unitOfWork.Notifications.GetAsync(
                    x=>x.OrderByDescending(x=>x.CreatedAt),
                    null, x => x.UserId == userId));
            if (!notifications.Any())
                throw new NotFoundException(nameof(NotificationsDTO));

            var readNotifications = _mapper.Map<IEnumerable<ReadNotificationsDTO>>(
                await _unitOfWork.ReadNotifications.GetAsync(null, null, x => x.UserId == userId));

            notifications = notifications.Select(n =>
            {
                if (readNotifications.Any(x => x.NotificationId == n.Id))
                    n.ReadNotificationStatus = NotificationStatusDTO.Read;
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
            if (notificationModel.UserId != null && result)
            {

                await _hubContext.Clients.Group(notificationModel.UserId.ToString()).BroadcastMessage();
            }

            return result;
        }

        public async Task GenerateNotificationAsync(CreateNotificationDTO notificationModel)
        {
            var notification = _mapper.Map<Notifications>(notificationModel);

            await _unitOfWork.Notifications.InsertAsync(notification);
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
