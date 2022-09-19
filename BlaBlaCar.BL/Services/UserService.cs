﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.DTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.BL.Exceptions;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace BlaBlaCar.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISaveFileService _fileService;
        private readonly HostSettings _hostSettings;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserService(IUnitOfWork unitOfWork,
            IMapper mapper, ISaveFileService fileService, IOptionsSnapshot<HostSettings> hostSettings, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _contextAccessor = contextAccessor;
            _hostSettings = hostSettings.Value;
        }
        public async Task<UserDTO> GetUserInformationAsync(Guid currentUserId)
        {
            var user = _mapper.Map<UserDTO>(
                await _unitOfWork.Users.GetAsync(x => x.Include(i => i.Cars)
                        .ThenInclude(i => i.CarDocuments)
                        .Include(i => i.UserDocuments)
                        .Include(x => x.TripUsers)
                        .Include(x=>x.Trips),
                    u => u.Id == currentUserId)
            );
            if (user is null)
                throw new NotFoundException(nameof(UserDTO));

            if (user.UserImg != null)
                user.UserImg = user.UserImg.Insert(0, _hostSettings.CurrentHost);
            
            user.UserDocuments = user.UserDocuments.Select(x =>
               {
                   x.DrivingLicense = _hostSettings.CurrentHost + x.DrivingLicense;
                   return x;

               }).ToList();

            user.Cars = user.Cars.Select(x =>
               {
                   x.CarDocuments.Select(c =>
                   {
                       c.TechnicalPassport = _hostSettings.CurrentHost + c.TechnicalPassport;
                       return c;
                   }).ToList();
                   return x;
               }).ToList();
            return user;
        }

        public async Task<UserDTO> GetUserByIdAsync(Guid id)
        {
            var user = _mapper.Map<UserDTO>(await _unitOfWork.Users.GetAsync(
                x => x.Include(x => x.UserDocuments)
                    .Include(x => x.Cars)
                    .ThenInclude(x => x.CarDocuments)
                    .Include(x => x.Trips)
                    .Include(x => x.TripUsers),
                x => x.Id == id));
            if (user is null)
                throw new NotFoundException(nameof(UserDTO));

            if (user.UserImg != null)
                user.UserImg = user.UserImg.Insert(0, _hostSettings.CurrentHost);

            user.UserDocuments = user.UserDocuments.Select(x =>
            {
                x.DrivingLicense = _hostSettings.CurrentHost + x.DrivingLicense;
                return x;

            }).ToList();

            user.Cars = user.Cars.Select(x =>
            {
                x.CarDocuments.Select(c =>
                {
                    c.TechnicalPassport = _hostSettings.CurrentHost + c.TechnicalPassport;
                    return c;
                }).ToList();
                return x;
            }).ToList();

            return user;
        }

        public async Task<IEnumerable<UserDTO>> SearchUsersAsync(string userNameOrEmail)
        {
            var users = _mapper.Map<IEnumerable<UserDTO>>(
                await _unitOfWork.Users.GetAsync(
                    null, 
                    x=>x.Include(x=>x.TripUsers).Include(x=>x.Trips), 
                    x=>x.FirstName.Contains(userNameOrEmail) || 
                       x.Email.Contains(userNameOrEmail) || x.PhoneNumber.Contains(userNameOrEmail))
                );
            if (!users.Any()) throw new NoContentException();
            users = users.Select(u =>
            {
                if (u.UserImg != null)
                {
                    u.UserImg = u.UserImg.Insert(0, _hostSettings.CurrentHost);
                }
                return u;
            });

            return users;
        }

        

        public async Task<bool> RequestForDrivingLicense(Guid currentUserId, IEnumerable<IFormFile> drivingLicense)
        {
            var userModel = _mapper.Map<UserDTO>(await _unitOfWork.Users
                .GetAsync(null, x => x.Id == currentUserId));
            if (userModel.UserStatus == UserStatusDTO.Rejected) throw new PermissionException("This user cannot add driving license!");

           
            if (drivingLicense.Any())
            {
                var files = await _fileService.GetFilesDbPathListAsync(drivingLicense);

               userModel.UserDocuments = files.Select(f => new UserDocumentDTO() { User = userModel, DrivingLicense = f }).ToList();

                userModel.UserStatus = UserStatusDTO.Pending;


                var user = _mapper.Map<ApplicationUser>(userModel);
                _unitOfWork.Users.Update(user);
                return await _unitOfWork.SaveAsync(user.Id);
            }

            throw new NoFileException($"File is required!");
        }

        public async Task<bool> AddUserAsync(UserDTO user)
        {
            var userDTO =_mapper.Map<UserDTO>( await _unitOfWork.Users
                .GetAsync(null, x => x.Id == user.Id));
            if (userDTO != null) return true;
            
            await _unitOfWork.Users.InsertAsync(_mapper.Map<ApplicationUser>(user));
            return await _unitOfWork.SaveAsync(user.Id);
        }

        public async Task<bool> UpdateUserAsync(UpdateUserDTO newUserData, Guid currentUserId)
        {
            var accessToken = _contextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            StringContent userModel = new StringContent(JsonConvert.SerializeObject(newUserData), Encoding.UTF8, "application/json");

            using var response = await httpClient.PostAsync(_hostSettings.IdentityServerUpdateUserHost, userModel);
            if (response.IsSuccessStatusCode)
            {
                var user = await _unitOfWork.Users.GetAsync(null, x => x.Id == newUserData.Id);

                user.FirstName = newUserData.FirstName;
                user.Email = newUserData.Email;
                user.PhoneNumber = newUserData.PhoneNumber;

                _unitOfWork.Users.Update(user);
                return await _unitOfWork.SaveAsync(currentUserId);
            }

            throw new PermissionException("This user cannot update data");
        }

        public async Task<bool> UpdateUserImgAsync(IFormFile userImg, Guid currentUserId)
        {
            if (userImg is null) throw new NoFileException($"File is required!");
            var user = _mapper.Map<UserDTO>(
                await _unitOfWork.Users.GetAsync(null, x => x.Id == currentUserId));

            var img = await _fileService.GetFilesDbPathListAsync(userImg);

            user.UserImg = img;
            
            _unitOfWork.Users.Update(_mapper.Map<ApplicationUser>(user));
            return await _unitOfWork.SaveAsync(currentUserId);

        }

       
    }
}
