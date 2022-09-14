using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.ChatDTOs
{
    public class UsersInChatsDTO:BaseDTO
    {
        public Guid UserId { get; set; }
        public UserDTO User { get; set; }
        public Guid ChatId { get; set; }
        public ChatDTO Chat { get; set; }
        public UserRoleInChatTypeDTO Role { get; set; }
    }
}
