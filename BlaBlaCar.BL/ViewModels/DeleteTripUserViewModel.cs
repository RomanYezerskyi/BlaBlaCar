using BlaBlaCar.BL.ODT.TripModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.ViewModels
{
    public class DeleteTripUserViewModel
    {
        public Guid Id { get; set; }
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public Guid UserId { get; set; }
        public ICollection<TripUserModel>? TripUsers { get; set; }
    }
}
