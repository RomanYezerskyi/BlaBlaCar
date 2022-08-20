using AutoMapper;
using BlaBlaCar.BL.ODT;
using BlaBlaCar.BL.ODT.BookTripModels;
using BlaBlaCar.BL.ODT.CarModels;
using BlaBlaCar.BL.ODT.NotificationModels;
using BlaBlaCar.BL.ODT.TripModels;
using BlaBlaCar.BL.ViewModels;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Entities.CarEntities;
using BlaBlaCar.DAL.Entities.NotificationEntities;
using BlaBlaCar.DAL.Entities.TripEntities;

namespace BlaBlaCar.BL
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserModel>().ReverseMap();

            CreateMap<NewTripViewModel, TripModel>().ReverseMap();
            CreateMap<Trip, TripModel>().ReverseMap();
            CreateMap<TripAndTripUsersViewModel, TripModel>().ReverseMap();

            CreateMap<AvailableSeats, AvailableSeatsModel>().ReverseMap();
            CreateMap<Car, CarModel>().ReverseMap();
            CreateMap<NewCarViewModel, CarModel>().ReverseMap();

            CreateMap<AvailableSeatsModel, AvailableSeatViewModel>().ReverseMap();
            CreateMap<SeatModel, Seat>().ReverseMap();
            CreateMap<TripUser, TripUserModel>().ReverseMap();
            CreateMap<AddNewBookTrip, TripUserModel>().ReverseMap();
            CreateMap<TripUserViewModel, TripUserModel>().ReverseMap();

            CreateMap<CarDocuments, CarDocumentsModel>().ReverseMap();
            CreateMap<UserDocuments, UserDocumentsModel>().ReverseMap();
            
            CreateMap<Notification, NotificationModel>().ReverseMap();
            CreateMap<ReadNotification, ReadNotificationModel>().ReverseMap();
            CreateMap<Notification, CreateNotificationViewModel>().ReverseMap();
            CreateMap<ReadNotification, CreateNotificationViewModel>();
            CreateMap<Notification, GetNotificationViewModel>().ReverseMap();

        }
    }
}
