using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.ODT.BookTripModels
{
    public class BookedSeatModel
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        //public SeatModel Seat { get; set; }
        public int? BookedTripId { get; set; }
        public BookedTripModel? BookedTrip { get; set; }
    }
}
