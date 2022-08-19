using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public class CarDocuments:BaseEntity
    {
        //public Guid Id { get; set; }
        public string TechPassport { get; set; }
        public Guid CarId { get; set; }
        public Car Car { get; set; }

    }
}
