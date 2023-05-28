using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.TripDTOs
{
    public class SearchTripsResponseDTO
    {
        public IEnumerable<TripDTO> Trips { get; set; }
        public int TotalTrips { get; set; }
    }
}
