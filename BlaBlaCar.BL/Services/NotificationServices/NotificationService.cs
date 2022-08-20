using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.NotificationModels;
using BlaBlaCar.BL.ViewModels;
using BlaBlaCar.DAL.Entities.NotificationEntities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;

namespace BlaBlaCar.BL.Services.NotificationServices
{
    public class NotificationService: INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetNotificationViewModel>> GetUserNotificationsAsync(ClaimsPrincipal principal)
        {
            var userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var notifications = _mapper.Map<IEnumerable<GetNotificationViewModel>>(
                await _unitOfWork.Notifications.GetAsync(
                    x=>x.OrderByDescending(x=>x.CreatedAt),
                    null, x => x.UserId == userId));
            var readNotifications = _mapper.Map<IEnumerable<ReadNotificationModel>>(
                await _unitOfWork.ReadNotifications.GetAsync(null, null, x => x.UserId == userId));

            notifications = notifications.Select(n =>
            {
                if (readNotifications.Any(x => x.NotificationId == n.Id))
                    n.ReadNotificationStatus = ReadNotificationStatus.Read;
                return n;
            });
            return notifications;
        }
        public async Task<bool> CreateNotificationAsync(CreateNotificationViewModel notificationModel, ClaimsPrincipal principal)
        {
            var notification = _mapper.Map<Notification>(notificationModel);

            await _unitOfWork.Notifications.InsertAsync(notification);

            var createdBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(createdBy);
        }

        public async Task ChangeUserStatusNotificationAsync(CreateNotificationViewModel notificationModel)
        {
            var notification = _mapper.Map<Notification>(notificationModel);

            await _unitOfWork.Notifications.InsertAsync(notification);
        }

        public async Task<bool> ReadAllNotificationAsync(IEnumerable<NotificationModel> notification, ClaimsPrincipal principal)
        {
            var readNotifications = new List<ReadNotificationModel>();


            readNotifications.AddRange(
                notification.Select(n => new ReadNotificationModel() { NotificationId = n.Id, UserId = (Guid)n.UserId }));

            await _unitOfWork.ReadNotifications.InsertRangeAsync(_mapper.Map<IEnumerable<ReadNotification>>(readNotifications));

            var createdBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(createdBy);
        }
    }
}
