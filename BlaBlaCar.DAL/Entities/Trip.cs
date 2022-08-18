
namespace BlaBlaCar.DAL.Entities
{
    public class Trip:BaseEntity
    {
        //public Guid Id { get; set; }
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PricePerSeat { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid CarId { get; set; }
        public Car Car { get; set; }
        public ICollection<TripUser> TripUsers { get; set; }
        public ICollection<AvailableSeats> AvailableSeats { get; set; }
    }
}
