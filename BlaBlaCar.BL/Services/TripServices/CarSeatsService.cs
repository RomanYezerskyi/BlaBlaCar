using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Interfaces;

namespace BlaBlaCar.BL.Services.TripServices
{
    public class CarSeatsService : ICarSeatsService
    {
        public CarSeatsService()
        {
        }
        public CarDTO AddSeatsToCarAsync(CarDTO carModel, int count)
        {
            carModel.Seats = new List<SeatDTO>();
            for (int i = 1; i <= count; i++)
            {
                carModel.Seats.Add(new SeatDTO() { Car = carModel, SeatNumber = i });
            }

            return carModel;
        }
    }
}
