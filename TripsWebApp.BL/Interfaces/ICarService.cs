﻿using System;
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
        Task<IEnumerable<CarDTO>> GetUserCarsAsync(Guid currentUserId);
        Task<CarDTO> GetCarByIdAsync(Guid id);
        Task AddCarAsync(CreateCarDTO carModel, Guid currentUserId);
        Task<bool> UpdateCarAsync(UpdateCarDTO carModel, Guid currentUserId);
        Task<bool> UpdateCarDocumentsAsync(UpdateCarDocumentsDTO carModel, Guid currentUserId);
        Task<bool> DeleteCarAsync(Guid carId, Guid userId);
    }
}
