﻿using AutoMapper;
using BlaBlaCar.BL.DTOs.AdminDTOs;
using BlaBlaCar.BL.DTOs.BookTripDTOs;
using BlaBlaCar.BL.DTOs.BookTripModels;
using BlaBlaCar.BL.DTOs.CarDTOs;
using BlaBlaCar.BL.DTOs.ChatDTOs;
using BlaBlaCar.BL.DTOs.FeedbackDTOs;
using BlaBlaCar.BL.DTOs.NotificationDTOs;
using BlaBlaCar.BL.DTOs.TripDTOs;
using BlaBlaCar.BL.DTOs.UserDTOs;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Entities.CarEntities;
using BlaBlaCar.DAL.Entities.ChatEntities;
using BlaBlaCar.DAL.Entities.NotificationEntities;
using BlaBlaCar.DAL.Entities.TripEntities;

namespace BlaBlaCar.BL
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
            CreateMap<UsersStatisticsDTO, ApplicationUser>().ReverseMap();

            CreateMap<FeedBack, FeedBackDTO>().ReverseMap();
            CreateMap<CreateFeedbackDTO, FeedBackDTO>().ReverseMap();

            CreateMap<CreateTripDTO, TripDTO>().ReverseMap();
            CreateMap<Trip, TripDTO>().ReverseMap();
            CreateMap<GetTripWithTripUsersDTO, TripDTO>().ReverseMap();

            CreateMap<AvailableSeats, AvailableSeatDTO>().ReverseMap();
            CreateMap<Car, CarDTO>().ReverseMap();
            CreateMap<CreateCarDTO, CarDTO>().ReverseMap();
            CreateMap<CarStatisticsDTO, Car>().ReverseMap();
            CreateMap<CarDTO, UpdateCarDTO>().ReverseMap();

            CreateMap<AvailableSeatDTO, NewAvailableSeatDTO>().ReverseMap();
            CreateMap<SeatDTO, Seat>().ReverseMap();
            CreateMap<TripUser, TripUserDTO>().ReverseMap();
            CreateMap<NewBookTripModel, TripUserDTO>().ReverseMap();
            CreateMap<UpdateTripUserDTO, TripUserDTO>().ReverseMap();

            CreateMap<CarDocuments, CarDocumentDTO>().ReverseMap();
            CreateMap<UserDocuments, UserDocumentDTO>().ReverseMap();
            
            CreateMap<Notifications, NotificationsDTO>().ReverseMap();
            CreateMap<ReadNotifications, ReadNotificationsDTO>().ReverseMap();
            CreateMap<Notifications, CreateNotificationDTO>().ReverseMap();
            CreateMap<ReadNotifications, CreateNotificationDTO>();
            CreateMap<Notifications, GetNotificationsDTO>().ReverseMap();
            CreateMap<NotificationsDTO, GetNotificationsDTO>().ReverseMap();

            CreateMap<Chat, ChatDTO>().ReverseMap();
            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<ChatParticipant, ChatParticipantDTO>().ReverseMap();
            CreateMap<Message, CreateMessageDTO> ().ReverseMap();
            CreateMap<ReadMessages, ReadMessagesDTO>().ReverseMap();
        }
    }
}
