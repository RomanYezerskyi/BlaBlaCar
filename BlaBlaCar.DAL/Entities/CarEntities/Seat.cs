using BlaBlaCar.DAL.Entities.TripEntities;

namespace BlaBlaCar.DAL.Entities.CarEntities
{
    public class Seat : BaseEntity
    {
        //public Guid Id { get; set; }
        public int Num { get; set; }
        public Guid CarId { get; set; }
        public Car Car { get; set; }
        public ICollection<TripUser> TripUsers { get; set; }
        public ICollection<AvailableSeats> AvailableSeats { get; set; }
    }
}
