using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.DTOs.ChatDTOs;
using BlaBlaCar.BL.Exceptions;
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
        public async Task<IEnumerable<ChatDTO>> GetUserChatsAsync(Guid currentUserId)
        {
            var chats = await _unitOfWork.Chats.GetAsync(null,
                x => x.Include(x => x.Users.Where(x => x.UserId != currentUserId))
                    .ThenInclude(x => x.User)
                    .Include(x => x.Messages.OrderByDescending(x => x.CreatedAt).Take(1)),
                x => x.Users.Any(x => x.UserId == currentUserId));

            var chatsDTOs = _mapper.Map<IEnumerable<ChatDTO>>(chats);
            if (!chatsDTOs.Any()) return null;

            foreach (var dto in chatsDTOs)
            {
                foreach (var chat in chats)
                {
                    if (chat.Messages.FirstOrDefault().ChatId == dto.Id)
                        dto.LastMessage = _mapper.Map<MessageDTO>(chat.Messages.FirstOrDefault());
                }
                if (dto.LastMessage != null)
                {
                    var readMessage = await _unitOfWork.ReadMessages.GetAsync(null, x => x.MessageId == dto.LastMessage.Id 
                        && x.UserId == currentUserId);
                    if (readMessage != null) dto.LastMessage.Status = MessageStatus.Read;
                }
            }
            chatsDTOs = chatsDTOs.Select(c =>
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
            return chatsDTOs.OrderByDescending(x=>x.LastMessage.CreatedAt).ThenBy(x=> x.LastMessage.Status == MessageStatus.Read);
        }

        public async Task<int> IsUnreadMessagesAsync(Guid currentUserId)
        {
            var messages =_mapper.Map<IEnumerable<MessageDTO>>(await _unitOfWork.Messages.GetAsync(null, null,
                x => x.Chat.Users.Any(x => x.UserId == currentUserId)));
            var readMessages = _mapper.Map<IEnumerable<ReadMessagesDTO>>(
                await _unitOfWork.ReadMessages.GetAsync(null, null, x => x.UserId == currentUserId));

            var result = messages.Count(m => readMessages.All(rm => rm.MessageId != m.Id));
            return result;
        }
        public async Task<Guid> CreatePrivateChatAsync(Guid userId, Guid currentUserId)
        {
            var checkIfChatExist =await GetPrivateChatAsync(userId, currentUserId);
            if (checkIfChatExist != Guid.Empty) return checkIfChatExist;
            var chatDto = new ChatDTO()
            {
                Type = ChatTypeDTO.Private,
            };
            var chatParticipants = new List<ChatParticipantDTO>()
            {
                new DTOs.ChatDTOs.ChatParticipantDTO() { Chat = chatDto, UserId = userId, Role = UserRoleInChatTypeDTO.Member},
                new DTOs.ChatDTOs.ChatParticipantDTO() { Chat = chatDto, UserId = currentUserId, Role = UserRoleInChatTypeDTO.Owner},
            };
            chatDto.Users = chatParticipants;
            var chat = _mapper.Map<Chat>(chatDto);
            await _unitOfWork.Chats.InsertAsync(chat);
            await _unitOfWork.SaveAsync(currentUserId);
            return chat.Id;
        }
        public async Task<Guid> GetPrivateChatAsync(Guid userId, Guid currentUserId)
        {
            var chat = _mapper.Map<ChatDTO>(await _unitOfWork.Chats.GetAsync(null,
                x => x.Users.Any(x => x.UserId == currentUserId) &&
                     x.Users.Any(x => x.UserId == userId) && x.Type == ChatType.Private));
            if(chat != null) return chat.Id;
            return Guid.Empty;
        }
        public async Task<ChatDTO> GetChatByIdAsync(Guid chatId)
        {
            var chat = _mapper.Map<ChatDTO>(await _unitOfWork.Chats.GetAsync(
                x=>x.Include(x=>x.Users).ThenInclude(x=>x.User),
                x => x.Id == chatId));
            var readMessages =
              await  _unitOfWork.ReadMessages.GetAsync(null, null, x => x.ChatId == chatId);
           chat.Users = chat.Users.Select(u =>
           {
               if(u.User.UserImg != null)
                   u.User.UserImg = _hostSettings.CurrentHost + u.User.UserImg;
               return u;
           }).ToList();

            return chat;
        }

        public async Task<IEnumerable<MessageDTO>> GetChatMessages(Guid chatId, Guid currentUserId, int take, int skip)
        {
            var messages = _mapper.Map<IEnumerable<MessageDTO>>(
                await _unitOfWork.Messages.GetAsync(x=>x.OrderByDescending(x=>x.CreatedAt), 
                    null, x => x.ChatId == chatId, take:take,skip:skip));
            if (!messages.Any()) return messages;

            var readMessages =
                await _unitOfWork.ReadMessages.GetAsync(null, null, x => x.ChatId == chatId && x.UserId == currentUserId);
            messages = messages.Select(m =>
            {
                if (readMessages.Any(x => x.MessageId == m.Id))
                {
                    m.Status = MessageStatus.Read;
                }

                return m;
            });
            return messages.Reverse().ToList();
        }

        public async Task<bool> ReadMessagesFromChat(IEnumerable<MessageDTO> messages, Guid currentUserId)
        {
            var readMessages = messages.Select(m => 
                new ReadMessagesDTO() {Id = Guid.NewGuid(),MessageId = m.Id, ChatId = m.ChatId, UserId = currentUserId});
            await _unitOfWork.ReadMessages.InsertRangeAsync(_mapper.Map<IEnumerable<ReadMessages>>(readMessages));
            return await _unitOfWork.SaveAsync(currentUserId);
        }

        public async Task<bool> CreateMessageAsync(CreateMessageDTO messageModel, Guid currentUserId)
        {
            var newMessage = new MessageDTO()
            {
                ChatId = messageModel.ChatId,
                Text = messageModel.Text,
                UserId = currentUserId,
                CreatedAt = DateTimeOffset.Now
            };
            var message = _mapper.Map<Message>(newMessage);
            await _unitOfWork.Messages.InsertAsync(message);
            var res =  await _unitOfWork.SaveAsync(currentUserId);
            if (!res) return res;
            newMessage.User = messageModel.User;
            //newMessage.User.UserImg = _hostSettings.CurrentHost + messageModel.User.UserImg;
            newMessage.Id = message.Id;
            await _chatHubService.NotifyChat(newMessage.ChatId, newMessage);
            _backgroundJobs.Enqueue<IChatHubService>(
                s => s.NotifyAllChatUsers(newMessage.ChatId, currentUserId));

            return res;
        }

    }
}
