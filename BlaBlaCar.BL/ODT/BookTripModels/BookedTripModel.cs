using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.ODT.BookTripModels
{
    public class BookedTripModel
    {
        public int? Id { get; set; }
        public int CountOfSeats { get; set; }
        public string? UserId { get; set; }
        public UserModel User { get; set; }
        public int TripId { get; set; }
        public TripModel Trip { get; set; }
        public ICollection<BookedSeatModel>? BookedSeats { get; set; }

    }
}
