using BlaBlaCar.BL.DTOs.ChatDTOs;
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
        Task<Guid> CreatePrivateChatAsync(Guid userId, Guid currentUserId);
        Task<IEnumerable<ChatDTO>> GetUserChatsAsync(Guid currentUserId);
        Task<bool> CreateMessageAsync(CreateMessageDTO messageModel, Guid currentUserId);
        Task<ChatDTO> GetChatByIdAsync(Guid chatId);
        Task<IEnumerable<MessageDTO>> GetChatMessages(Guid chatId, int take, int skip);
        Task<bool> ReadMessagesFromChat(IEnumerable<MessageDTO> messages, Guid currentUserId);
    }
}
