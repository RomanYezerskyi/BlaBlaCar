﻿using BlaBlaCar.BL.DTOs.ChatDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IChatService
    {
        Task<Guid> CreatePrivateChatAsync(Guid userId, ClaimsPrincipal principal);
        Task<IEnumerable<ChatDTO>> GetUserChatsAsync(ClaimsPrincipal principal);
        Task<bool> CreateMessageAsync(CreateMessageDTO messageModel, ClaimsPrincipal principal);
        Task<ChatDTO> GetChatByIdAsync(Guid chatId);
        Task<bool> ReadMessagesFromChat(IEnumerable<MessageDTO> messages, ClaimsPrincipal principal);
    }
}