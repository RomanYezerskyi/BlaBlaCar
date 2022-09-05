using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BlaBlaCar.BL.Hubs
{
    public class ChatHub: Hub<IChatHubClient>
    {
        public async Task<string> GetChatConnectionId(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
            return Context.ConnectionId;
        }
    }
}
