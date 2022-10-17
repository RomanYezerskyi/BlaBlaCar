
using BlaBlaCar.BL.DTOs.CarDTOs;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ICarSeatsService
    {
        CarDTO AddSeatsToCarAsync(CarDTO carModel, int count);
    }
}
