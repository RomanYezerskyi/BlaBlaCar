using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.FeedbackDTOs;
using BlaBlaCar.BL.DTOs.NotificationDTOs;

namespace BlaBlaCar.BL.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<GetNotificationsDTO>> GetUserUnreadNotificationsAsync(ClaimsPrincipal principal);
        Task<IEnumerable<GetNotificationsDTO>> GetUserNotificationsAsync(ClaimsPrincipal principal, int take, int skip);
        Task<bool> CreateNotificationAsync(CreateNotificationDTO notificationModel, ClaimsPrincipal principal);
        Task GenerateNotificationAsync(CreateNotificationDTO notificationModel);
        Task<bool> ReadAllNotificationAsync(IEnumerable<NotificationsDTO> notification, ClaimsPrincipal principal);
        Task<IEnumerable<GetNotificationsDTO>> GetGlobalNotificationsAsync(int take, int skip);
        Task<IEnumerable<GetNotificationsDTO>> GetUsersNotificationsAsync(int take, int skip);
        Task GenerateFeedBackNotificationAsync(Guid tripId);
        Task AddFeedBack(CreateFeedbackDTO newFeedback, ClaimsPrincipal principal);
    }
}
