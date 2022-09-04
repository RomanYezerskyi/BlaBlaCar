using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.TripDTOs;

namespace BlaBlaCar.BL.DTOs.AdminDTOs
{
    public class TripsStatisticsDTO:TripDTO
    {
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
