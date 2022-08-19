using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ViewModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface INotificationService
    {
        Task<bool> CreateNotificationAsync(NotificationViewModel notificationModel, ClaimsPrincipal principal);
        Task<bool> ReadNotificationAsync(NotificationViewModel notificationModel, ClaimsPrincipal principal);
    }
}
