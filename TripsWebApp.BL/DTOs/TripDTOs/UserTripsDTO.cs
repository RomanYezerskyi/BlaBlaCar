using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.TripDTOs
{
    public class UserTripsDTO
    {
        public IEnumerable<GetTripWithTripUsersDTO> Trips { get; set; }
        public int TotalTrips { get; set; }
    }
}
