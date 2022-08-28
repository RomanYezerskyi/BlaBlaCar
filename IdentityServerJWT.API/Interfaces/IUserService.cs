using IdentityServerJWT.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerJWT.API.Interfaces
{
    public interface IUserService
    {
        Task<List<IdentityRole>> GetRoles();
        public Task<UserWithRolesModel> GetUser(string email);
        Task<IdentityResult> ChangeUserRole(UserChangeRoleModel roleModel);
        Task<IdentityResult> UpdateUser(UpdateUser userModel);
        Task<IdentityResult> UpdateUserPassword(UpdateUserPassword newPasswordModel);
    }
}
