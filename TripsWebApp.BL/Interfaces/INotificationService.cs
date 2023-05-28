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
        Task<IEnumerable<GetNotificationsDTO>> GetUserUnreadNotificationsAsync(Guid currentUserId);
        Task<IEnumerable<GetNotificationsDTO>> GetUserNotificationsAsync(int take, int skip, Guid currentUserId);
        Task<bool> CreateNotificationAsync(CreateNotificationDTO notificationModel, Guid currentUserId);
        Task GenerateNotificationAsync(CreateNotificationDTO notificationModel, Guid createdBy);
        Task<bool> ReadAllNotificationAsync(IEnumerable<NotificationsDTO> notification, Guid currentUserId);
        Task<IEnumerable<GetNotificationsDTO>> GetGlobalNotificationsAsync(int take, int skip);
        Task<IEnumerable<GetNotificationsDTO>> GetUsersNotificationsAsync(int take, int skip);
        Task GenerateFeedBackNotificationAsync(Guid tripId);
        Task AddFeedBack(CreateFeedbackDTO newFeedback, Guid currentUserId);
        Task<IEnumerable<FeedBackDTO>> GetUserFeedBacks(Guid currentUserId, int take, int skip);
    }
}
