namespace BlaBlaCar.BL.DTOs.MapDTOs;

public class UserLocation
{
    public double Lat { get; set; }
    public double Lon { get; set; }
    public bool Passanger { get; set; }
    public string UserId { get; set; }
    public Guid TripId { get; set; }
}