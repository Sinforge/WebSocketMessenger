using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSocketMessenger.Core.Dtos;
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
    [HttpPost]
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(401)]
    public async Task<Guid> CreateGroupAsync([FromBody] CreateGroupDto dto)
    {
        Guid clientId = GetUserIdFromClaims();
        return await _groupService.CreateGroupAsync(clientId, dto.Name);
    }

    [Authorize]
    [HttpPost("invite")]
    public async Task<IEnumerable<UserToInviteDto>> GetUsersToInviteInGroupAsync([FromBody] GetUsersToInviteRequest request)
    {
        return await _groupService.GetUserToInviteInGroup(request.Id, request.SearchString);
    }

    [Authorize]
    [HttpPost("invite/users")]
    public async Task AddUserToGroupAsync([FromBody] AddUsersToGroupRequest request)
    {
        await _groupService.AddUsersToGroupAsync(request.Ids, request.GroupId);
    }

    [Authorize]
    [HttpPost("members")]
    public async Task<IEnumerable<GroupMemberDto>> GetGroupMembersAsync([FromBody] GetGroupMembersRequest request)
    {
        return await _groupService.GetGroupMembersAsync(request.Id);
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