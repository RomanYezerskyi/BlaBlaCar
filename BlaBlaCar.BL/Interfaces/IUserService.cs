﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders.Physical;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetUserInformationAsync(ClaimsPrincipal claimsPrincipal);
        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<IEnumerable<UserModel>> SearchUsersAsync(UserModel model);
        Task<bool> RequestForDrivingLicense(ClaimsPrincipal principal, IEnumerable<IFormFile> drivingLicense);
        Task<bool> СheckIfUserExistsAsync(ClaimsPrincipal user);
        Task<bool> AddUserAsync(ClaimsPrincipal user);
        Task<bool> UpdateUserAsync(UpdateUserModel userModel, ClaimsPrincipal principal);
        Task<bool> UpdateUserImgAsync(IFormFile userImg, ClaimsPrincipal principal);
        Task<bool> DeleteUserAsync(int id);
    }
}
