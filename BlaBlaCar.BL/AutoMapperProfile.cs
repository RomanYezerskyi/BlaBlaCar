using AutoMapper;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.DAL.Entities;

namespace BlaBlaCar.BL
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserModel>().ReverseMap();
            //.ForMember(u => u.UserName, opt =>
            //    opt.MapFrom(ur => ur.Email));
            CreateMap<TripModel, AddNewTripModel>().ReverseMap();
            CreateMap<AddNewTripModel, TripModel>().ReverseMap();
            CreateMap<Trip, TripModel>().ReverseMap();

            CreateMap<AvailableSeats, AvailableSeatsModel>().ReverseMap();
            CreateMap<Car, CarModel>().ReverseMap();
            CreateMap<AddNewCarModel, CarModel>().ReverseMap();

            CreateMap<Seat, SeatModel>().ReverseMap();
            CreateMap<TripUser, TripUserModel>().ReverseMap();
            //CreateMap<BookedSeat, BookedSeatModel>().ReverseMap();

            //CreateMap<BookedTrip, BookedTripModel>().ReverseMap();
            //CreateMap<BookedTrip, AddNewBookTrip>().ReverseMap();
            //CreateMap<AddNewBookTrip, BookedTripModel>().ReverseMap();

        }
    }
}
