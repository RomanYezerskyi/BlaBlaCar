using BlaBlaCar.BL.DTOs.ChatDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IChatHubService
    {
        Task NotifyChat(Guid chatId, MessageDTO message);
        Task NotifyAllChatUsers(Guid chatId, Guid currentUserId);
    }
}
