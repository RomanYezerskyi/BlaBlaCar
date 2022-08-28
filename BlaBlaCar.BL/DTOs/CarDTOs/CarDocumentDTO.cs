namespace BlaBlaCar.BL.DTOs.CarDTOs
{
    public class CarDocumentDTO
    {
        public Guid Id { get; set; }
        public string TechPassport { get; set; }
        public Guid CarId { get; set; }
        public CarDTO Car { get; set; }

    }
}
