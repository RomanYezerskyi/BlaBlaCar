namespace BlaBlaCar.BL.ODT.TripModels
{
    public class SearchTripModel
    {
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public DateTime StartTime { get; set; }
        public int CountOfSeats { get; set; }
    }
}
