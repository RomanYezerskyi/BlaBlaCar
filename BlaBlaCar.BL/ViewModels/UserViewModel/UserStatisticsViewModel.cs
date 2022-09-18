using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.ViewModels.UserViewModel
{
    public class UserStatisticsViewModel
    {
        public IEnumerable<int> TripsStatisticsCount { get; set; }
        public IEnumerable<DateTime> TripsDateTime { get; set; }
    }
}
