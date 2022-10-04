using BlaBlaCar.BL.DTOs.MapDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IMapService
    {
        Task<MapRequestResult?> GetPlaceInformation(double latitude, double longitude);
    }
}
