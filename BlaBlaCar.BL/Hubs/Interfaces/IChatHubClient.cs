using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.ChatDTOs;

namespace BlaBlaCar.BL.Hubs.Interfaces
{
    public interface IChatHubClient
    {
        Task BroadcastChatMessage(MessageDTO message);
        Task BroadcastMessagesFromChats(Guid chatId);
    }
}
