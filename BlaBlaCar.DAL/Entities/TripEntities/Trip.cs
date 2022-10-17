using System.ComponentModel.DataAnnotations.Schema;
using BlaBlaCar.DAL.Entities.CarEntities;
using NetTopologySuite.Geometries;

namespace BlaBlaCar.DAL.Entities.TripEntities
{
    public class Trip : BaseEntity
    {
        public Point? StartLocation { get; set; }
        public Point? EndLocation { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int PricePerSeat { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid? CarId { get; set; }
        public Car? Car { get; set; }
        public ICollection<TripUser> TripUsers { get; set; }
        public ICollection<AvailableSeats> AvailableSeats { get; set; }

        [NotMapped]
        public TimeSpan TripTime => EndTime - StartTime;
    }
}
