﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.ChatDTOs;

namespace BlaBlaCar.BL.Hubs
{
    public interface IChatHubClient
    {
        Task BroadcastChatMessage(MessageDTO message);
        Task BroadcastMessagesFormAllChats(Guid chatId);
    }
}
