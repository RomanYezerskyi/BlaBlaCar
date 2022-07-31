using AutoMapper;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, ApplicationUser>();
            //.ForMember(u => u.UserName, opt =>
            //    opt.MapFrom(ur => ur.Email));
            CreateMap<Trip, TripModel>().ReverseMap();
            CreateMap<Trip, AddNewTripModel>().ReverseMap();
            CreateMap<AddNewTripModel, TripModel>().ReverseMap();

            CreateMap<BookedSeat, BookedSeatModel>().ReverseMap();

            CreateMap<BookedTrip, BookedTripModel>().ReverseMap();
            CreateMap<BookedTrip, AddNewBookTrip>().ReverseMap();
            CreateMap<AddNewBookTrip, BookedTripModel>().ReverseMap();

            CreateMap<Seat, SeatModel>().ReverseMap();
        }
    }
}
