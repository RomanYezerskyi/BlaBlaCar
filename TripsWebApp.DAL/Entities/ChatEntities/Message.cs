namespace BlaBlaCar.DAL.Entities.ChatEntities
{
    public class Message:BaseEntity
    {
        public string Text { get; set; }
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
