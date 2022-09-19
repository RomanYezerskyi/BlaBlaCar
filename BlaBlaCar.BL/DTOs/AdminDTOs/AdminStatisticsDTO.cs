using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.AdminDTOs
{
    public class AdminStatisticsDTO
    {
        public IEnumerable<int> UsersStatisticsCount { get; set; }
        public IEnumerable<DateTime> UsersDateTime { get; set; }

        public IEnumerable<int> CarsStatisticsCount { get; set; }
        public IEnumerable<DateTime> CarsDateTime { get; set; }

        public IEnumerable<int> TripsStatisticsCount { get; set; }
        public IEnumerable<DateTime> TripsDateTime { get; set; }

        public IEnumerable<int> WeekStatisticsTripsCount { get; set; }
        public IEnumerable<DayOfWeek> WeekTripsDateTime { get; set; }
    }
}
