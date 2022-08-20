using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.NotificationModels;
using BlaBlaCar.BL.ViewModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<GetNotificationViewModel>> GetUserNotificationsAsync(ClaimsPrincipal principal);
        Task<bool> CreateNotificationAsync(CreateNotificationViewModel notificationModel, ClaimsPrincipal principal);
        Task ChangeUserStatusNotificationAsync(CreateNotificationViewModel notificationModel);
        Task<bool> ReadAllNotificationAsync(IEnumerable<NotificationModel> notification, ClaimsPrincipal principal);
    }
}
