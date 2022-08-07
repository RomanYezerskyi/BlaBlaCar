using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.CarModels;

namespace BlaBlaCar.BL.ViewModels
{
    public class GetRequests
    {
        public ModelStatus UserStatus { get; set; }
        public CarModelStatus CarStatus { get; set; }
    }
}
