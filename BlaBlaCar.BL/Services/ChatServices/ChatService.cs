using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.DTOs.ChatDTOs;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Entities.ChatEntities;
using BlaBlaCar.DAL.Interfaces;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace BlaBlaCar.BL.Services.ChatServices
{
    public class ChatService:IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ChatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ChatDTO>> GetUserChatsAsync(ClaimsPrincipal principal)
        {
            var currentUserId = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id).Value);
            var chats = _mapper.Map<IEnumerable<ChatDTO>>(
                await _unitOfWork.Chats.GetAsync(null, 
                    x=>x.Include(x=>x.Users).ThenInclude(x=>x.User)
                        .Include(x=>x.Messages), 
                    x=>x.Users.Any(x=>x.UserId == currentUserId)));
           
            return chats;
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
            newChat.Users = usersInChat;

            await _unitOfWork.Chats.InsertAsync(_mapper.Map<Chat>(newChat));
            return await _unitOfWork.SaveAsync(currentUserId);
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
            return await _unitOfWork.SaveAsync(currentUserId);

        }

    }
}
