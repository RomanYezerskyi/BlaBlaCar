using Microsoft.Extensions.FileProviders.Physical;

namespace BlaBlaCar.BL.ODT.CarModels
{
    public class CarDocumentsModel
    {
        public Guid Id { get; set; }
        public string TechPassport { get; set; }
        public Guid CarId { get; set; }
        public CarModel Car { get; set; }

    }
}
