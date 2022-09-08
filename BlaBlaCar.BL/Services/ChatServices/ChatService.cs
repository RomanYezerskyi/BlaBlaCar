using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.DTOs.ChatDTOs;
using BlaBlaCar.BL.Hubs;
using BlaBlaCar.BL.Hubs.Interfaces;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Entities.ChatEntities;
using BlaBlaCar.DAL.Interfaces;
using Hangfire;
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
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IChatHubService _chatHubService;
        public ChatService(IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IOptionsSnapshot<HostSettings> hostSettings, 
            IBackgroundJobClient backgroundJobs, IChatHubService chatHubService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _backgroundJobs = backgroundJobs;
            _chatHubService = chatHubService;
            _hostSettings = hostSettings.Value;
        }
        public async Task<IEnumerable<ChatDTO>> GetUserChatsAsync(ClaimsPrincipal principal)
        {
            var currentUserId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var chats = _mapper.Map<IEnumerable<ChatDTO>>(
                await _unitOfWork.Chats.GetAsync(null, 
                    x=>x.Include(x=>x.Users.Where(x=>x.UserId != currentUserId))
                        .ThenInclude(x=>x.User)
                        .Include(x=>x.Messages.OrderByDescending(x=>x.CreatedAt).Take(1)),
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
        public async Task<Guid> CreatePrivateChatAsync(Guid userId, ClaimsPrincipal principal)
        {
            var checkIfChatExist =await GetPrivateChatAsync(userId, principal);
            if (checkIfChatExist != Guid.Empty) return checkIfChatExist;
            var currentUserId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var chatDto = new ChatDTO()
            {
                Type = ChatDTOType.Private,
            };
            var usersInChat = new List<UsersInChatsDTO>()
            {
                new UsersInChatsDTO() { Chat = chatDto, UserId = userId, Role = UserRoleInChatTypeDTO.Member},
                new UsersInChatsDTO() { Chat = chatDto, UserId = currentUserId, Role = UserRoleInChatTypeDTO.Owner},
            };
            chatDto.Users = usersInChat;
            var chat = _mapper.Map<Chat>(chatDto);
            await _unitOfWork.Chats.InsertAsync(chat);
            await _unitOfWork.SaveAsync(currentUserId);
            return chat.Id;
        }
        public async Task<Guid> GetPrivateChatAsync(Guid userId, ClaimsPrincipal principal)
        {
            var currentUserId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var chat = _mapper.Map<ChatDTO>(await _unitOfWork.Chats.GetAsync(null,
                x => x.Users.Any(x => x.UserId == currentUserId) &&
                     x.Users.Any(x => x.UserId == userId) && x.Type == ChatType.Private));
            if(chat != null) return chat.Id;
            return Guid.Empty;
        }
        public async Task<ChatDTO> GetChatByIdAsync(Guid chatId)
        {
            var chat = _mapper.Map<ChatDTO>(await _unitOfWork.Chats.GetAsync(
                x=>x.Include(x=>x.Messages).ThenInclude(x=>x.User)
                    .Include(x=>x.Users).ThenInclude(x=>x.User),
                x => x.Id == chatId));
            var readMessages =
              await  _unitOfWork.ReadMessages.GetAsync(null, null, x => x.ChatId == chatId);
            chat.Messages = chat.Messages.Select(m =>
            {
                if (m.User.UserImg != null)
                    m.User.UserImg = _hostSettings.CurrentHost + m.User.UserImg;

                if (readMessages.Any(x => x.MessageId == m.Id))
                {
                    m.Status = MessageStatus.Read;
                }
                return m;
            }).ToList();

            
            
            return chat;
        }

        public async Task<bool> ReadMessagesFromChat(IEnumerable<MessageDTO> messages, ClaimsPrincipal principal)
        {
            var userId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var readMessages = messages.Select(m => 
                new ReadMessagesDTO() { MessageId = m.Id, ChatId = m.ChatId, UserId = m.UserId });
            await _unitOfWork.ReadMessages.InsertRangeAsync(_mapper.Map<IEnumerable<ReadMessages>>(readMessages));
            return await _unitOfWork.SaveAsync(userId);
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

            await _chatHubService.NotifyChat(newMessage.ChatId, newMessage);
            _backgroundJobs.Enqueue<IChatHubService>(
                s => s.NotifyAllChatUsers(newMessage.ChatId, currentUserId));

            return res;
        }

    }
}
