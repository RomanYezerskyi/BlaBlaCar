using BlaBlaCar.BL.DTOs.UserDTOs;
namespace BlaBlaCar.BL.DTOs.FeedbackDTOs
{
    public class FeedBackDTO : BaseDTO
    {
        public string Text { get; set; }
        public int Rate { get; set; }
        public Guid UserId { get; set; }
        public UserDTO? User { get; set; }
    }
}
