using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSocketMessenger.Core.DTOs;
using WebSocketMessenger.Core.Interfaces.Services;

namespace WebSocketMessenger.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    
    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(401)]
    public async Task<Guid> CreateGroupAsync([FromBody] CreateGroupDto dto)
    {
        Guid clientId = GetUserIdFromClaims();
        return await _groupService.CreateGroupAsync(clientId, dto.Name);
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