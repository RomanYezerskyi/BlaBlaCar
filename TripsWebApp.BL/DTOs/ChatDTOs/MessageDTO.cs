﻿using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.ChatDTOs
{
    public class MessageDTO:BaseDTO
    {
       
        public string Text { get; set; }
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public UserDTO? User { get; set; }
        public MessageStatus Status { get; set; } = MessageStatus.Unread;
    }
}
