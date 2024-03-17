using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebSocketMessenger.Core.Interfaces.Services;
using WebSocketMessenger.Core.Models;

namespace WebSocketMessenger.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;

        }
        [HttpGet("conversation/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), 200)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<int>> GetConversationMessages([FromRoute] Guid userId)
        {
            Guid clientId = GetUserIdFromClaims();
            return await _messageService.GetMessagesByUsers(clientId, userId);
        }
        [HttpGet("group/{groupId}")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), 200)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<int>> GetGroupMessages([FromRoute] Guid groupId)
        {
            Guid clientId = GetUserIdFromClaims();
            return await _messageService.GetMessageByGroupAsync(clientId, groupId);

        }

        [HttpGet("{messageId}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(401)]
        public async Task<MessageDTO> GetMessageById([FromRoute] int messageId)
        {
            Guid clientId = GetUserIdFromClaims();
            return await _messageService.GetMessageByIdAsync(messageId, clientId);

        }





        [NonAction]
        private Guid GetUserIdFromClaims()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                foreach(Claim claim in claims) { 
                    if(claim.Type == "Id")
                    {
                        return Guid.Parse(claim.Value);
                    }
                }

            }
            throw new Exception("Cant find claim 'Id'");
        }
    }
}
