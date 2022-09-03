using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders.Physical;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserInformationAsync(ClaimsPrincipal claimsPrincipal);
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task<IEnumerable<UserDTO>> SearchUsersAsync(string userData);
        Task<bool> RequestForDrivingLicense(ClaimsPrincipal principal, IEnumerable<IFormFile> drivingLicense);
        Task<bool> СheckIfUserExistsAsync(ClaimsPrincipal user);
        Task<bool> AddUserAsync(ClaimsPrincipal user);
        Task<bool> UpdateUserAsync(UpdateUserDTO userModel, ClaimsPrincipal principal);
        Task<bool> UpdateUserImgAsync(IFormFile userImg, ClaimsPrincipal principal);
        Task<bool> DeleteUserAsync(int id);
    }
}
