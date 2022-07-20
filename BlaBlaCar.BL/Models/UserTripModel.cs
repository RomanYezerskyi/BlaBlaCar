
namespace BlaBlaCar.BL.Models
{
    public class UserTripModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public int TripId { get; set; }
        public TripModel Trip { get; set; }
    }
}
