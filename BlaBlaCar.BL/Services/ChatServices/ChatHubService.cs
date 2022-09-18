using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Hubs.Interfaces;
using BlaBlaCar.BL.Hubs;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using BlaBlaCar.BL.DTOs.ChatDTOs;

namespace BlaBlaCar.BL.Services.ChatServices
{
    public class ChatHubService:IChatHubService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub, IChatHubClient> _hubContext;
       
        public ChatHubService(IUnitOfWork unitOfWork, IMapper mapper,IHubContext<ChatHub, IChatHubClient> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
        }
        public async Task NotifyChat(Guid chatId, MessageDTO message)
        {
            await _hubContext.Clients.Groups(chatId.ToString()).BroadcastChatMessage(message);
        }

        public async Task NotifyAllChatUsers(Guid chatId, Guid currentUserId)
        {
            var chatUsers = await _unitOfWork.ChatParticipants.GetAsync(null, null,
                x => x.UserId != currentUserId && x.ChatId == chatId
            );
            foreach (var user in chatUsers)
            {
                await _hubContext.Clients.Groups(user.UserId.ToString())
                    .BroadcastMessagesFromChats(chatId);
            }
        }
    }
}
