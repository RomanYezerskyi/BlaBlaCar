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
        Task<UserDTO> GetUserInformationAsync(Guid currentUserId);
        Task<UserDTO> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserDTO>> SearchUsersAsync(string userNameOrEmail);
        Task<bool> RequestForDrivingLicense(Guid currentUserId, IEnumerable<IFormFile> drivingLicense);
        Task<bool> AddUserAsync(UserDTO user);
        Task<bool> UpdateUserAsync(UpdateUserDTO userModel, Guid currentUserId);
        Task<bool> UpdateUserImgAsync(IFormFile userImg, Guid currentUserId);
    }
}
