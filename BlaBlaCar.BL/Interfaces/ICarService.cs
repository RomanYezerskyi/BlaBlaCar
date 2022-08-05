using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ViewModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<CarModel>> GetUserCarsAsync(ClaimsPrincipal principal);
        Task<CarModel> GetCarByIdAsync(Guid id);
        Task<bool> AddCarAsync(NewCarViewModel carModel, ClaimsPrincipal principal);
    }
}
