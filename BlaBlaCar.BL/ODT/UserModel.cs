using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ODT.TripModels;
using Microsoft.Extensions.FileProviders.Physical;

namespace BlaBlaCar.BL.ODT
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public ModelStatus UserStatus { get; set; }
        public ICollection<UserDocumentsModel> UserDocuments { get; set; }
        public ICollection<CarModel> Cars { get; set; }
        public ICollection<TripModel> Trips { get; set; }
        public ICollection<TripUserModel> TripUsers { get; set; }
    }
}
