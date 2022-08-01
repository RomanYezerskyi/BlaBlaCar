using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL.ODT.TripModels
{
    public class AddNewTripModel
    {
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PricePerSeat { get; set; }
        public string Description { get; set; }
        public int CountOfSeats { get; set; }
        public int CarId { get; set; }

    }
}
