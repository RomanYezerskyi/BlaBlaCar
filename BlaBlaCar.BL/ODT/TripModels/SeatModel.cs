namespace BlaBlaCar.BL.ODT.TripModels
{
    public class SeatModel
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public int TripId { get; set; }
        public TripModel Trip { get; set; }
    }
}
