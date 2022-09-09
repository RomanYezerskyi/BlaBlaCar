

using BlaBlaCar.DAL.Entities.ChatEntities;
namespace BlaBlaCar.BL.DTOs.ChatDTOs
{
    
    public class ChatDTO:BaseEntity
    {
        public string ChatName { get; set; }
        public string ChatImage { get; set; }
        public ICollection<MessageDTO> Messages { get; set; }
        public ICollection<UsersInChatsDTO> Users { get; set; }
        public ChatDTOType Type { get; set; }

    }
}
