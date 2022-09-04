using BlaBlaCar.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserChats()
        {
            var chats = await _chatService.GetUserChatsAsync(User);
            if (chats.Any()) return Ok(chats);
            return BadRequest();
        }
        [HttpPost("private/{userId}")]
        public async Task<IActionResult> GetPrivateChat(Guid userId)
        {
            var chat = await _chatService.CreatePrivateChatAsync(userId, User);
            if (chat) return Ok(chat);
            return BadRequest();
        }
    }
}
