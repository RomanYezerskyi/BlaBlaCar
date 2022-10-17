namespace BlaBlaCar.DAL.Entities.ChatEntities
{
    public class ChatParticipant:BaseEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
        public UserRoleInChatType Role { get; set; }
    }
}
