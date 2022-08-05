using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.BL.ViewModels;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlaBlaCar.BL.Services
{
    public class CarSeatsService:ICarSeatsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CarSeatsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<CarModel> AddSeatsToCarAsync(CarModel carModel, int count)
        {
            carModel.Seats = new List<SeatModel>();
            for (int i = 1; i <= count; i++)
            {
                carModel.Seats.Add(new SeatModel() { Car = carModel, Num = i });
            }

            return Task.FromResult(carModel);
        }
    }
}
