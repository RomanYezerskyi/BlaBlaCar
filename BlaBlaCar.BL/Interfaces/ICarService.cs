using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.CarDTOs;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<CarDTO>> GetUserCarsAsync(ClaimsPrincipal principal);
        Task<CarDTO> GetCarByIdAsync(Guid id);
        Task AddCarAsync(CreateCarDTO carModel, ClaimsPrincipal principal);
        Task<bool> UpdateCarAsync(UpdateCarDTO carModel, ClaimsPrincipal principal);
        Task<bool> UpdateCarDocumentsAsync(UpdateCarDocumentsDTO carModel, ClaimsPrincipal principal);
    }
}
