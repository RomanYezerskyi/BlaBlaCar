using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.ChatDTOs
{
    public class ReadMessagesDTO:BaseDTO
    {
        public Guid MessageId { get; set; }
        public MessageDTO Message { get; set; }
        public Guid ChatId { get; set; }
        public ChatDTO Chat { get; set; }
        public Guid UserId { get; set; }
        public UserDTO User { get; set; }
    }
}
