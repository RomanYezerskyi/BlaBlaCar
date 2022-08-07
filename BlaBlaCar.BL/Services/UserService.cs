using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders.Physical;

namespace BlaBlaCar.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public Task<UserModel> GetUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserModel>> SearchUsersAsync(UserModel model)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> RequestForCar(ClaimsPrincipal principal, IFormFile drivingLicense)
        {
            var userEmail = principal.Identity.Name;
            var userModel = _mapper.Map<UserModel>(await _unitOfWork.Users
                .GetAsync(null, x => x.Email == userEmail));
            if (userModel.UserStatus == ModelStatus.Rejected) throw new Exception("This user cannot add driving license!");

           
            if (drivingLicense.Length > 0)
            {
                var folderName = Path.Combine("DriverDocuments", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = ContentDispositionHeaderValue.Parse(drivingLicense.ContentDisposition).FileName.Trim('"').Split(".");
                var newFileName = new string(Guid.NewGuid() +"."+ fileName.Last());
                var fullPath = Path.Combine(pathToSave, newFileName);
                var dbPath = Path.Combine(folderName, newFileName);
                await using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    drivingLicense.CopyTo(stream);
                }

                //await using (var stream = new FileStream(dbPath, FileMode.Open))
                //{
                //    PhysicalFileInfo fileInfo = new PhysicalFileInfo(new FileInfo(stream.Name));
                //    return fileInfo;
                //}
                userModel.DrivingLicense = dbPath;
                userModel.UserStatus = ModelStatus.Pending;


                var user = _mapper.Map<ApplicationUser>(userModel);
                _unitOfWork.Users.Update(user);
                return await _unitOfWork.SaveAsync();
            }

            throw new Exception("Problems with file");
        }
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }

        public async Task<bool> СheckIfUserExistsAsync(ClaimsPrincipal principal)
        {
            var userEmail = principal.Identity.Name;
            var user = await _unitOfWork.Users
                .GetAsync(null, x => x.Email == userEmail);
            if (user == null)
            {
                return await AddUserAsync(principal);
            }
            return true;
        }

        public async Task<bool> AddUserAsync(ClaimsPrincipal principal)
        {
            var user = new UserModel()
            {
                Id = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id)?.Value,
                Email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                FirstName = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value,
                PhoneNumber = principal.Claims.FirstOrDefault(x=>x.Type == JwtClaimTypes.PhoneNumber).Value,
                UserStatus = ModelStatus.WithoutCar
            };
            await _unitOfWork.Users.InsertAsync(_mapper.Map<ApplicationUser>(user));
            return await _unitOfWork.SaveAsync();
        }

        public Task<bool> UpdateUserAsync(UserModel user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
