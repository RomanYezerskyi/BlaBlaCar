using BlaBlaCar.BL.DTOs.ChatDTOs;
using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.AdminOrUser)]
    public class ChatController : CustomBaseController
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserChats()
        {
            var chats = await _chatService.GetUserChatsAsync(UserId);
            return Ok(chats);
        }
        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChatById(Guid chatId)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId);
            return Ok(chat);
        }
        [HttpGet("private/{userId}")]
        public async Task<IActionResult> GetPrivateChat(Guid userId)
        {
            var chat = await _chatService.CreatePrivateChatAsync(userId, UserId);
            return Ok(chat);
        }

        [HttpGet("chat-messages/{chatId}")]
        public async Task<IActionResult> GetChatMessages([FromRoute]Guid chatId,[FromQuery] int take,[FromQuery] int skip)
        {
            var chat = await _chatService.GetChatMessages(chatId, take, skip);
            return Ok(chat);
        }

        [HttpPut("read-messages")]
        public async Task<IActionResult> ReadChatMessages(IEnumerable<MessageDTO> messages)
        {
            var result = await _chatService.ReadMessagesFromChat(messages, UserId);
            return NoContent();
        }

        [HttpPost("message")]
        public async Task<IActionResult> CreateMessageChat(CreateMessageDTO message)
        {
            var res = await _chatService.CreateMessageAsync(message, UserId);
            return NoContent();
        }
    }
}
