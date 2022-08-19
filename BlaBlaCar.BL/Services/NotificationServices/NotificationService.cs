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

        public async Task<bool> CreateNotificationAsync(NotificationViewModel notificationModel, ClaimsPrincipal principal)
        {
            var notification = _mapper.Map<Notification>(notificationModel);

            await _unitOfWork.Notifications.InsertAsync(notification);

            var createdBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(createdBy);
        }

        public async Task<bool> ReadNotificationAsync(NotificationViewModel notificationModel, ClaimsPrincipal principal)
        {
            var readNotification = new ReadNotificationModel()
            {
                NotificationId = (Guid)notificationModel.Id,
                UserId = (Guid)notificationModel.UserId,
            };

            await _unitOfWork.ReadNotifications.InsertAsync(_mapper.Map<ReadNotification>(readNotification));

            var createdBy = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            return await _unitOfWork.SaveAsync(createdBy);
        }
    }
}
