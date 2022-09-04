using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.TripDTOs;

namespace BlaBlaCar.BL.ViewModels.AdminViewModels
{
    public class UserTripsViewModel
    {
        public IEnumerable<GetTripWithTripUsersDTO> Trips { get; set; }
        public int TotalTrips { get; set; }
    }
}
