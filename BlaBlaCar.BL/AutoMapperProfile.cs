using AutoMapper;
using BlaBlaCar.BL.DTOs.BookTripDTOs;
using BlaBlaCar.BL.DTOs.BookTripModels;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
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
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();

            CreateMap<CreateTripDTO, TripDTO>().ReverseMap();
            CreateMap<Trip, TripDTO>().ReverseMap();
            CreateMap<GetTripWithTripUsersDTO, TripDTO>().ReverseMap();

            CreateMap<AvailableSeats, AvailableSeatDTO>().ReverseMap();
            CreateMap<Car, CarDTO>().ReverseMap();
            CreateMap<CreateCarDTO, CarDTO>().ReverseMap();

            CreateMap<AvailableSeatDTO, NewAvailableSeatDTO>().ReverseMap();
            CreateMap<SeatDTO, Seat>().ReverseMap();
            CreateMap<TripUser, TripUserDTO>().ReverseMap();
            CreateMap<AddNewBookTripDTO, TripUserDTO>().ReverseMap();
            CreateMap<UpdateTripUserDTO, TripUserDTO>().ReverseMap();

            CreateMap<CarDocuments, CarDocumentDTO>().ReverseMap();
            CreateMap<UserDocuments, UserDocumentDTO>().ReverseMap();
            
            CreateMap<Notification, NotificationDTO>().ReverseMap();
            CreateMap<ReadNotification, ReadNotificationDTO>().ReverseMap();
            CreateMap<Notification, CreateNotificationDTO>().ReverseMap();
            CreateMap<ReadNotification, CreateNotificationDTO>();
            CreateMap<Notification, GetNotificationDTO>().ReverseMap();

        }
    }
}
