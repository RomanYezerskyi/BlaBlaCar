using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetUserAsync(int id);
        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<IEnumerable<UserModel>> SearchUsersAsync(UserModel model);
        Task<bool> СheckIfUserExistsAsync(ClaimsPrincipal user);
        Task<bool> AddUserAsync(ClaimsPrincipal user);
        Task<bool> UpdateUserAsync(UserModel user);
        Task<bool> DeleteUserAsync(int id);
    }
}
