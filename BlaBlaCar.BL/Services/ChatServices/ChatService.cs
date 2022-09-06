using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.DTOs.ChatDTOs;
using BlaBlaCar.BL.Hubs;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Entities.ChatEntities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlaBlaCar.BL.Services.ChatServices
{
    public class ChatService:IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HostSettings _hostSettings;
        private readonly IHubContext<ChatHub, IChatHubClient> _hubContext;
        public ChatService(IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<HostSettings> hostSettings, IHubContext<ChatHub, IChatHubClient> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
            _hostSettings = hostSettings.Value;
        }
        public async Task<IEnumerable<ChatDTO>> GetUserChatsAsync(ClaimsPrincipal principal)
        {
            var currentUserId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var chats = _mapper.Map<IEnumerable<ChatDTO>>(
                await _unitOfWork.Chats.GetAsync(null, 
                    x=>x.Include(x=>x.Users.Where(x=>x.UserId != currentUserId))
                        .ThenInclude(x=>x.User)
                        .Include(x=>x.Messages.Take(1)),
                    x=>x.Users.Any(x=>x.UserId == currentUserId)));
            chats = chats.Select(c =>
            {
                c.Users = c.Users.Select(u =>
                {
                    if (u.User.UserImg != null)
                    {
                        u.User.UserImg = _hostSettings.CurrentHost + u.User.UserImg;
                    }
                    return u;
                }).ToList();
                return c;
            });
            return chats.ToList();
        }
        public async Task<bool> CreatePrivateChatAsync(Guid userId, ClaimsPrincipal principal)
        {

            var currentUserId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var newChat = new ChatDTO()
            {
                Type = ChatDTOType.Private,
            };
            var usersInChat = new List<UsersInChatsDTO>()
            {
                new UsersInChatsDTO() { Chat = newChat, UserId = userId, Role = UserRoleInChatTypeDTO.Member},
                new UsersInChatsDTO() { Chat = newChat, UserId = currentUserId, Role = UserRoleInChatTypeDTO.Owner},
            };
            //newChat.Users = usersInChat;

            await _unitOfWork.Chats.InsertAsync(_mapper.Map<Chat>(newChat));
            return await _unitOfWork.SaveAsync(currentUserId);
        }
        public async Task<ChatDTO> GetPrivateChatAsync(Guid userId, ClaimsPrincipal principal)
        {
            var currentUserId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var chat = _mapper.Map<ChatDTO>(await _unitOfWork.Chats.GetAsync(null,
                x => x.Users.Any(x => x.UserId == currentUserId) &&
                     x.Users.Any(x => x.UserId == userId) && x.Type == ChatType.Private));
            return chat;
        }
        public async Task<ChatDTO> GetChatByIdAsync(Guid chatId)
        {
            var chat = _mapper.Map<ChatDTO>(await _unitOfWork.Chats.GetAsync(
                x=>x.Include(x=>x.Messages).ThenInclude(x=>x.User)
                    .Include(x=>x.Users).ThenInclude(x=>x.User),
                x => x.Id == chatId));

            chat.Messages = chat.Messages.Select(m =>
            {
                if (m.User.UserImg != null)
                    m.User.UserImg = _hostSettings.CurrentHost + m.User.UserImg;
                return m;
            }).ToList();
            return chat;
        }
        public async Task<bool> CreateMessageAsync(CreateMessageDTO messageModel, ClaimsPrincipal principal)
        {
            var currentUserId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var newMessage = new MessageDTO()
            {
                ChatId = messageModel.ChatId,
                Text = messageModel.Text,
                UserId = currentUserId,
               
            };
            await _unitOfWork.Messages.InsertAsync(_mapper.Map<Message>(newMessage));
            var res =  await _unitOfWork.SaveAsync(currentUserId);
            if (!res) return res;
            newMessage.User = messageModel.User;
            newMessage.User.UserImg = _hostSettings.CurrentHost + messageModel.User.UserImg;
            await _hubContext.Clients.Groups(messageModel.ChatId.ToString()).BroadcastChatMessage(newMessage);
            //await _hubContext.Clients.Groups(currentUserId.ToString()).BroadcastMessagesFormAllChats();
            var chatUsers = await _unitOfWork.UsersInChats.GetAsync(null, null,
                x => x.UserId != currentUserId && x.ChatId == messageModel.ChatId
            );
            foreach (var user in chatUsers)
            {
                await _hubContext.Clients.Groups(user.UserId.ToString())
                    .BroadcastMessagesFormAllChats(newMessage.ChatId);
            }
            return res;
        }

    }
}
