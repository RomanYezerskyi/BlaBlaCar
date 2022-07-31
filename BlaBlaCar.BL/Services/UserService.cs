using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;

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

        public async Task<bool> СheckIfUserExistsAsync(ClaimsPrincipal principal)
        {
            var userEmail = principal.Identity.Name;
            var user = await _unitOfWork.Users
                .GetAsync(null, x => x.Email == userEmail);
            if (user == null)
            {
                await AddUserAsync(principal);
                return await _unitOfWork.SaveAsync();
            }
            return true;
        }

        public async Task<bool> AddUserAsync(ClaimsPrincipal principal)
        {
            var user = new ApplicationUser()
            {
                Id = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id)?.Value,
                UserName = principal.Identity.Name,
                Email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                FirstName = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value,
                PhoneNumber = principal.Claims.FirstOrDefault(x=>x.Type == JwtClaimTypes.PhoneNumber).Value
            };
            await _unitOfWork.Users.InsertAsync(user);
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
