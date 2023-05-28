using System.ComponentModel.DataAnnotations;

namespace BlaBlaCar.BL.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
