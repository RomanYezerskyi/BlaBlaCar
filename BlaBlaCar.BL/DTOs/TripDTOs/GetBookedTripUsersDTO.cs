using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.TripDTOs
{
    public class GetBookedTripUsersDTO
    {
        public Guid UserId { get; set; }
        public UserDTO User { get; set; }
        public IEnumerable<SeatDTO> Seats { get; set; }
    }
}
