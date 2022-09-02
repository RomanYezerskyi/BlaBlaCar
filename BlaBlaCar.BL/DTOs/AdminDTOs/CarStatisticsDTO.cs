using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.AdminDTOs
{
    public class CarStatisticsDTO
    {
        public Guid Id { get; set; }
        public string ModelName { get; set; }
        public string RegistNum { get; set; }
        public CarTypeDTO CarType { get; set; }
        public Guid UserId { get; set; }
        public UserDTO User { get; set; }
        public ICollection<CarDocumentDTO> CarDocuments { get; set; }
        public CarDTOStatus CarStatus { get; set; }
        public ICollection<SeatDTO> Seats { get; set; }
        public ICollection<TripDTO> Trips { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
