using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.NotificationDTOs;

namespace BlaBlaCar.BL.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<GetNotificationsDTO>> GetUserNotificationsAsync(ClaimsPrincipal principal);
        Task<bool> CreateNotificationAsync(CreateNotificationDTO notificationModel, ClaimsPrincipal principal);
        Task GenerateNotificationAsync(CreateNotificationDTO notificationModel);
        Task<bool> ReadAllNotificationAsync(IEnumerable<NotificationsDTO> notification, ClaimsPrincipal principal);
    }
}
