using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ViewModels;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlaBlaCar.BL.Services.Admin
{
    public class AdminService: IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AdminService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserModel>> GetRequestsAsync(ModelStatus status)
        {
            var users = _mapper.Map<IEnumerable<UserModel>>(await _unitOfWork.Users.GetAsync(null,
                x=>
                    x.Include(x=>x.Cars.Where(x=>x.CarStatus == (Status)status)),
                x=>x.UserStatus == (Status)status || x.Cars.Any(c=>c.CarStatus == (Status)status)));
            return users;

        }
      
        public async Task<UserModel> GetUserRequestsAsync(string id)
        {
            var user = _mapper.Map<UserModel>(await _unitOfWork.Users.GetAsync(
                x => x.Include(x => x.Cars),
                x => x.Id == id));
            return user;
        }
        public async Task<bool> ChangeUserStatusAsync(ChangeUserStatus changeUserStatus)
        {
            var user = _mapper.Map<UserModel>(
                await _unitOfWork.Users.GetAsync(null, x => x.Id == changeUserStatus.UserId));

            if (user is null) throw new Exception("User not found");
            user.UserStatus = changeUserStatus.Status;

            //добавити меседжі
            return await _unitOfWork.SaveAsync();
        }
        public async Task<bool> ChangeCarStatusAsync(ChangeCarStatus changeCarStatus)
        {
            var car = _mapper.Map<CarModel>
                (await _unitOfWork.Cars.GetAsync(null, x=>x.Id == changeCarStatus.CarId));

            if (car is null) throw new Exception("Car not found");
           car.CarStatus = changeCarStatus.Status;

            //добавити меседжі
            return await _unitOfWork.SaveAsync();
        }
    }
}
