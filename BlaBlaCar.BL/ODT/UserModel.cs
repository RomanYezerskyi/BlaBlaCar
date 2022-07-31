using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.ODT
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public ICollection<TripModel> UserTrips { get; set; }
        public ICollection<BookedTripModel> BookedTrips { get; set; }
    }
}
