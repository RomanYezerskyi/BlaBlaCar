using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.ODT;

namespace BlaBlaCar.BL.ViewModels
{
    public class ChangeUserStatus
    {
        public Guid UserId { get; set; }
        public ModelStatus Status { get; set; }
    }
}
