namespace BlaBlaCar.BL.DTOs
{
    public class BaseDTO
    {
        public virtual Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; } 
    }
}
