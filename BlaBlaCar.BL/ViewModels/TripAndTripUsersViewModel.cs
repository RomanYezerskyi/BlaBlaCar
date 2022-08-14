using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.ViewModels
{
    public class TripAndTripUsersViewModel: TripModel
    {
        public List<BookedTripUsersViewModel> BookedTripUsers { get; set; }
    }
}
