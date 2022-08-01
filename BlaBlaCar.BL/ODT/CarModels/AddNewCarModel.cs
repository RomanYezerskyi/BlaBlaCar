using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT.TripModels;

namespace BlaBlaCar.BL.ODT.CarModels
{
    public class AddNewCarModel
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string RegistNum { get; set; }
        public int CountOfSeats { get; set; }
        public CarTypeModel CarType { get; set; }
    }
}
