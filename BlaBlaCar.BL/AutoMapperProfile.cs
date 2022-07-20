using AutoMapper;
using BlaBlaCar.BL.Models;
using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, User>();
            //.ForMember(u => u.UserName, opt =>
            //    opt.MapFrom(ur => ur.Email));
            CreateMap<Trip, TripModel>().ReverseMap();
            CreateMap<UserTrip, UserTripModel>().ReverseMap();
            CreateMap<BookedSeat, BookedSeatModel>().ReverseMap();
            CreateMap<BookedTrip, BookedTripModel>().ReverseMap();
            CreateMap<Seat, SeatModel>().ReverseMap();
        }
    }
}
