using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.CarModels;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<CarModel>> GetUserCars(ClaimsPrincipal principal);
        Task<bool> AddCarAsync(AddNewCarModel carModel, ClaimsPrincipal principal);
    }
}
