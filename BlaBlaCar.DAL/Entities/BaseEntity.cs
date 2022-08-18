namespace BlaBlaCar.DAL.Entities
{
    public class BaseEntity
    {
        public virtual Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; } 
    }
}
