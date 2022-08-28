using Microsoft.Extensions.FileProviders.Physical;

namespace BlaBlaCar.BL.DTOs.UserDTOs
{
    public class UserDocumentDTO
    {
        public Guid Id { get; set; }
        public string DrivingLicense { get; set; }
        public Guid UserId { get; set; }
        public UserDTO User { get; set; }
    }
}
