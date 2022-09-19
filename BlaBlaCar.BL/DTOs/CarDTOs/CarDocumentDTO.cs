namespace BlaBlaCar.BL.DTOs.CarDTOs
{
    public class CarDocumentDTO:BaseDTO
    {
        public string TechnicalPassport { get; set; }
        public Guid CarId { get; set; }
        public CarDTO Car { get; set; }

    }
}
