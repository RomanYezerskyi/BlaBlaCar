using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL.ViewModels
{
    public class NewTripViewModel
    {
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PricePerSeat { get; set; }
        public string Description { get; set; }
        public int CountOfSeats { get; set; }
        public Guid CarId { get; set; }
        public ICollection<AvailableSeatViewModel> AvailableSeats { get; set; }

    }
}
