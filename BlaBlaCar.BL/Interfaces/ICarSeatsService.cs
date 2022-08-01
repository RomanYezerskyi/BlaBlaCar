using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ICarSeatsService
    {
        Task<CarModel> AddSeatsToCarAsync(CarModel carModel, int count);
        Task<TripModel> AddAvailableSeatsAsync(TripModel tripModel, int count);
    }
}
