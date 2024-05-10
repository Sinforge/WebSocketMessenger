using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebSocketMessenger.Core.Dtos;
using WebSocketMessenger.Core.Interfaces.Services;
using WebSocketMessenger.Core.Models;
using WebSocketMessenger.Infrastructure.FileSystem;

namespace WebSocketMessenger.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        public MessageController(IMessageService messageService, IUserService userService)
        {
            _userService = userService;
            _messageService = messageService;

        }
        [HttpGet("conversation/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), 200)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<MessageDto>> GetConversationMessages([FromRoute] Guid userId)
        {
            Guid clientId = GetUserIdFromClaims();
            return await _messageService.GetMessagesByUsers(clientId, userId);
        }

        [HttpGet("conversation")]
        public async Task<IEnumerable<DialogItemDto>> GetUserDialogs()
        {
            Guid clientId = GetUserIdFromClaims();
            return await _messageService.GetUserDialogs(clientId);
        }

        [HttpGet("group")]
        public async Task<IEnumerable<GroupItemDto>> GetUserGroups()
        {
            Guid clientId = GetUserIdFromClaims();
            return await _userService.GetUserGroupsAsync(clientId);
        }
        
        [HttpGet("group/{groupId}")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), 200)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<MessageDto>> GetGroupMessages([FromRoute] Guid groupId)
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


        [HttpGet("file/{messageId}")]

        public async Task GetFileByIdAsync([FromRoute] int messageId)
        {
            Guid clientId = GetUserIdFromClaims();
            var message = await _messageService.GetMessageByIdAsync(messageId, clientId);
            var file = FileManager.GetFileByFileName(message.Message);
            var separatorIndex = message.Message.IndexOf('_');
            var fileName = message.Message.Substring(separatorIndex + 1, message.Message.Length - separatorIndex - 1);
            var encodedFileName = WebUtility.UrlEncode(fileName);

            HttpContext.Response.Headers.ContentDisposition = $"attachment; filename={encodedFileName}";

            await HttpContext.Response.SendFileAsync(file);
            
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
