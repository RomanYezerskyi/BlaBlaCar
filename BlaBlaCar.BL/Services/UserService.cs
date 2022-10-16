using System;
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
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace BlaBlaCar.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly HostSettings _hostSettings;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserService(IUnitOfWork unitOfWork,
            IMapper mapper, IFileService fileService, IOptionsSnapshot<HostSettings> hostSettings, IHttpContextAccessor contextAccessor)
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
                });
                   return x;
               }).ToList();
            return user;
        }

        public async Task<UserDTO> GetUserByIdAsync(Guid id)
        {
            var user = _mapper.Map<UserDTO>(await _unitOfWork.Users.GetAsync(
                x => x.Include(x => x.UserDocuments)
                    .Include(x => x.Cars.OrderBy(x=>x.CarStatus))
                    .ThenInclude(x => x.CarDocuments).Include(x=>x.Cars).ThenInclude(x=>x.Seats)
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


            foreach (var car in user.Cars)
            {
                car.CarDocuments = car.CarDocuments.Select(d =>
                {
                    if (d.TechnicalPassport != null)
                        d.TechnicalPassport = _hostSettings.CurrentHost + d.TechnicalPassport;
                    return d;
                }).ToList();
            }

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
            if (!users.Any()) return null;
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


        public async Task<IEnumerable<UserDocumentDTO>> GetUserDocumentsAsync(Guid userId)
        {
            var documents = _mapper.Map<IEnumerable<UserDocumentDTO>>(
                await _unitOfWork.UserDocuments.
                    GetAsync(null, null, x=>x.UserId == userId)
                );
            foreach (var document in documents)
            {
                document.DrivingLicense = _hostSettings.CurrentHost + document.DrivingLicense;
            }
            return documents;
        }
        public async Task<bool> RequestForDrivingLicense(UpdateUserDocuments model, Guid currentUserId)
        {
            var userModel = _mapper.Map<UserDTO>(await _unitOfWork.Users
                .GetAsync(x=>x.Include(x=>x.UserDocuments), x => x.Id == currentUserId));
            if (userModel.UserStatus == UserStatusDTO.Rejected) throw new PermissionException("This user cannot add driving license!");

           
            if (model.DocumentsFile != null)
            {
                var files = await _fileService.GetFilesDbPathAsync(model.DocumentsFile);

                userModel.UserDocuments = files.Select(f => new UserDocumentDTO() { User = userModel, DrivingLicense = f }).ToList();

                userModel.UserStatus = UserStatusDTO.Pending;
                var user = _mapper.Map<ApplicationUser>(userModel);
                _unitOfWork.Users.Update(user);
                
            }

            if (model.DeletedDocuments != null)
            {
                var documents = userModel.UserDocuments.Select(x =>
                {
                    return model.DeletedDocuments.Any(d => d == x.Id.ToString()) ? x : null;
                }).Where(x=>x != null).ToList();
                _unitOfWork.UserDocuments.Delete(_mapper.Map<IEnumerable<UserDocuments>>(documents));
                _fileService.DeleteFilesFormApi(documents.Where(x => x != null).Select(x => x.DrivingLicense));
            }

            return await _unitOfWork.SaveAsync(currentUserId);
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

        public async Task<string> UpdateUserImgAsync(IFormFile userImg, Guid currentUserId)
        {
            if (userImg is null) throw new NoFileException($"File is required!");
            var user = _mapper.Map<UserDTO>(
                await _unitOfWork.Users.GetAsync(null, x => x.Id == currentUserId));

            var img = await _fileService.GetFileDbPathAsync(userImg);

            if(user.UserImg != null) _fileService.DeleteFilesFormApi(user.UserImg);

            user.UserImg = img;
            _unitOfWork.Users.Update(_mapper.Map<ApplicationUser>(user));
            await _unitOfWork.SaveAsync(currentUserId);
            var imageLink = _hostSettings.CurrentHost + img;
            return imageLink;

        }

       
    }
}
