using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace BlaBlaCar.BL.Hubs
{
    public class ChatHub: Hub<IChatHubClient>
    {
        public async Task JoinToChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }
        public async Task JoinToChatMessagesNotifications(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
    }
}
