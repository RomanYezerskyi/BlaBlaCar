using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.ODT.CarModels
{
    public class CarModel
    {
        public Guid Id { get; set; }
        public string ModelName { get; set; }
        public string RegistNum { get; set; }
        public CarTypeModel CarType { get; set; }
        public Guid UserId { get; set; }
        public UserModel User { get; set; }
        public ICollection<CarDocumentsModel> CarDocuments { get; set; }
        public ModelStatus CarStatus { get; set; }
        public ICollection<SeatModel> Seats { get; set; }
        public ICollection<TripModel> Trips { get; set; }
    }
}
