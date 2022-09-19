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

namespace BlaBlaCar.BL.Interfaces
{
    public interface IAdminService
    {
        Task<UserRequestsDTO> GetRequestsAsync(int take, int skip, UserStatusDTO status);
      
        Task<bool> ChangeUserStatusAsync(ChangeUserStatusDTO changeUserStatus, Guid currentUserId);
        Task<bool> ChangeCarStatusAsync(ChangeCarStatus changeCarStatus, Guid currentUserId);
        Task<AdminStatisticsDTO> GetStatisticsDataAsync(DateTimeOffset searchDate);
        Task<IEnumerable<UsersStatisticsDTO>> GetTopUsersListAsync(int take, int skip, UsersListOrderByType orderBy);
    }
}
