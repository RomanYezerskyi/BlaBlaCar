using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.TripDTOs;

namespace BlaBlaCar.BL.ViewModels
{
    public class UserBookedTripsViewModel
    {
        public IEnumerable<TripDTO> Trips { get; set; }
        public int TotalTrips { get; set; }
    }
}
