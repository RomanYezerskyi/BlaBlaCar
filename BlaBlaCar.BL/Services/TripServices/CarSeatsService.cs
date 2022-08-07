using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.DAL.Interfaces;

namespace BlaBlaCar.BL.Services.TripServices
{
    public class CarSeatsService : ICarSeatsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CarSeatsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public CarModel AddSeatsToCarAsync(CarModel carModel, int count)
        {
            carModel.Seats = new List<SeatModel>();
            for (int i = 1; i <= count; i++)
            {
                carModel.Seats.Add(new SeatModel() { Car = carModel, Num = i });
            }

            return carModel;
        }
    }
}
