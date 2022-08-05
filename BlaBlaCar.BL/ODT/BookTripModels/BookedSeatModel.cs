using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.ODT.BookTripModels
{
    public class BookedSeatModel
    {
        public Guid Id { get; set; }
        public int Num { get; set; }
        public Guid CarId { get; set; }
    }
}
