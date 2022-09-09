namespace BlaBlaCar.DAL.Entities.ChatEntities
{
    
    public class Chat:BaseEntity
    {
        public string? ChatName { get; set; }
        public string? ChatImage { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<UsersInChats> Users { get; set; }
        public ChatType Type { get; set; }

    }
}
