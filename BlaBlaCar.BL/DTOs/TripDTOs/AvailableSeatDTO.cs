using BlaBlaCar.BL.DTOs.CarDTOs;

namespace BlaBlaCar.BL.DTOs.TripDTOs
{
    public class AvailableSeatDTO
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public TripDTO Trip { get; set; }
        public Guid SeatId { get; set; }
        public SeatDTO Seat { get; set; }
        public AvailableSeatTypeDTO AvailableSeatsType { get; set; }
    }
}
