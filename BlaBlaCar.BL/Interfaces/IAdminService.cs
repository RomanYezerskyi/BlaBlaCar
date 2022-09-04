using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs;
using BlaBlaCar.BL.DTOs.AdminDTOs;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.ViewModels.AdminViewModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IAdminService
    {
        Task<UserRequestsViewModel> GetRequestsAsync(int take, int skip, UserDTOStatus status);
      
        Task<bool> ChangeUserStatusAsync(ChangeUserStatusDTO changeUserStatus, ClaimsPrincipal principal);
        Task<bool> ChangeCarStatusAsync(ChangeCarStatusDTO changeCarStatus, ClaimsPrincipal principal);
        Task<AdminStaticViewModel> GetStatisticsDataAsync(DateTimeOffset searchDate);
        Task<IEnumerable<UsersStatisticsDTO>> GetTopUsersListAsync(int take, int skip, UsersListOrderByType orderBy);
    }
}
