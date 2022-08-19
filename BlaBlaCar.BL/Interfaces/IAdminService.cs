using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ViewModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<UserModel>> GetRequestsAsync(ModelStatus status);
        Task<UserModel> GetUserRequestsAsync(Guid id);
        Task<bool> ChangeUserStatusAsync(ChangeUserStatus changeUserStatus, ClaimsPrincipal principal);
        Task<bool> ChangeCarStatusAsync(ChangeCarStatus changeCarStatus, ClaimsPrincipal principal);
    }
}
