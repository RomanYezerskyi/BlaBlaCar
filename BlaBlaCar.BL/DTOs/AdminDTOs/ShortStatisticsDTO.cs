using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.AdminDTOs
{
    public class ShortStatisticsDTO
    {
        public int UsersCount { get; set; }
        public int CarsCount { get; set; }
        public int TripsCount { get; set; }
        public int WeekTripsCount { get; set; }
    }
}
