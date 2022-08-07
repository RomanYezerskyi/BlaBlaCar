using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL.ViewModels
{
    public class ChangeCarStatus
    {
        public Guid CarId { get; set; }
        public ModelStatus Status { get; set; }
    }
}
