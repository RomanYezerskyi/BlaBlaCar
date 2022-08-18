using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.CarModels;

namespace BlaBlaCar.BL.ViewModels
{
    public class BookedTripUsersViewModel
    {
        public Guid UserId { get; set; }
        public UserModel User { get; set; }
        public IEnumerable<SeatModel> Seats { get; set; }
    }
}
