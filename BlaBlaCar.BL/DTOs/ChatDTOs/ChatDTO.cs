namespace BlaBlaCar.BL.DTOs.ChatDTOs
{
    
    public class ChatDTO:BaseDTO
    {
        public string ChatName { get; set; }
        public string ChatImage { get; set; }
        public ICollection<ChatParticipantDTO> Users { get; set; }
        public ChatTypeDTO Type { get; set; }

    }
}
