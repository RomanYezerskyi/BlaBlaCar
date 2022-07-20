using System.Security.Authentication;
using AutoMapper;
using BlaBlaCar.BL.Models;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlaBlaCar.BL.Services
{
    public class AuthorizationService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public AuthorizationService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, 
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> RegisterAsync(UserRegisterModel userRegisterModel)
        {
            var user = _mapper.Map<UserRegisterModel, User>(userRegisterModel);

            var userCreateResult = await _userManager.CreateAsync(user, userRegisterModel.Password);

            if (userCreateResult.Succeeded)
            {
                //await _userManager.AddToRoleAsync(user, "USER");
                return true;
            }

            return false;
        }
        public async Task<bool> Login(UserRegisterModel model)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (user is null)
            {
                //var roles = await _userManager.GetRolesAsync(user);
                //throw new AuthorizationException();
                throw new AuthenticationException("Email or password is incorrect.");
            }

            //var userSigninResult = await _userManager.CheckPasswordAsync(user, model.Password);
            var userSigninResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
            if (userSigninResult.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                //var token = GenerateJwt(user, roles);
                //return new AuthSettings(user, roles.FirstOrDefault(), token);
                return true;
            }

            throw new AuthenticationException("Email or password is incorrect.");
        }
        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                //throw new AuthorizationException();
                throw new("no name of role");
            }

            var newRole = new IdentityRole
            {
                Name = roleName
            };

            var roleResult = await _roleManager.CreateAsync(newRole);

            if (roleResult.Succeeded)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> AddUserToRoleAsync(string userEmail, string roleName)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == userEmail);
            if (user is null)
            {
                // throw new AuthorizationException();
                throw new("No user");
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
        //public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    var userList = new List<AuthSettings>();
        //    foreach (var user in users)
        //    {
        //        var roles = await _userManager.GetRolesAsync(user);
        //        AuthSettings authUser = new AuthSettings(user, roles.FirstOrDefault());
        //        userList.Add(authUser);
        //    }
        //    return userList;
        //}
        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                //throw new AuthorizationException("User not found");
                throw new("User not found");
            }
            var userRes = _mapper.Map<User>(user);
            return userRes;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                //throw new AuthorizationException();
                throw new("User not found");
            }

            var resUser = _mapper.Map<User>(user);
            return resUser;
        }
        //public async Task<AuthSettings> UpdateUserAsync(User user)
        //{
        //    var resUser =
        //        await _userManager.Users.SingleOrDefaultAsync(u =>
        //            u.Id == user.Id);
        //    if (resUser is null)
        //    {
        //        //throw new AuthorizationException();
        //        throw new("No user");
        //    }

        //    if (!string.IsNullOrEmpty(user.PasswordHash))
        //    {
        //        user.PasswordHash = _userManager.PasswordHasher.HashPassword(resUser, user.PasswordHash);
        //    }
        //    if (resUser.UserName != user.UserName)
        //    {
        //        resUser.UserName = user.UserName;
        //    }

        //    if (resUser.Email != user.Email)
        //    {
        //        resUser.Email = user.Email;
        //    }
        //    if (resUser.PhoneNumber != user.PhoneNumber)
        //    {
        //        resUser.PhoneNumber = user.PhoneNumber;
        //    }
        //    if (resUser.LastName != user.LastName)
        //    {
        //        resUser.LastName = user.LastName;
        //    }

        //    await _userManager.UpdateAsync(resUser);
        //    var roles = await _userManager.GetRolesAsync(resUser);
        //    var token = GenerateJwt(resUser, roles);
        //    return new AuthSettings(resUser, roles.First(), token);
        //}

        //public async Task DeleteUserByIdAsync(string id)
        //{
        //    var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
        //    var reservation = await _unitOfWork.Reservations.GetAsync(null, null, x => x.UserId == user.Id);
        //    if (reservation != null)
        //    {
        //        foreach (var reserv in reservation)
        //        {
        //            var booked =
        //                await _unitOfWork.BookedSeats.GetAsync(null, null, x => x.ReservationId == reserv.Id);

        //            if (booked.ToList().Count > 0)
        //                _unitOfWork.BookedSeats.Delete(booked);
        //        }
        //        _unitOfWork.Reservations.Delete(reservation);
        //    }

        //    if (user is null)
        //    {
        //        //throw new AuthorizationException();
        //        throw new("No user");
        //    }
        //    await _userManager.DeleteAsync(user);
        //    await _unitOfWork.SaveAsync();
        //}
        public async Task<IEnumerable<string>> GetUserRolesAsync(string email)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user is null)
            {
                //throw new AuthorizationException("User not found");
                throw new("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }
    }
}
