namespace BlaBlaCar.DAL.Entities.CarEntities
{
    public class CarDocuments : BaseEntity
    {
        //public Guid Id { get; set; }
        public string TechnicalPassport { get; set; }
        public Guid CarId { get; set; }
        public Car Car { get; set; }

    }
}
