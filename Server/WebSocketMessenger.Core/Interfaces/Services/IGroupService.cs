using WebSocketMessenger.Core.Dtos;
using WebSocketMessenger.Core.DTOs;

namespace WebSocketMessenger.Core.Interfaces.Services;

public interface IGroupService
{
    Task<Guid> CreateGroupAsync(Guid userId, string groupName);

    Task<IEnumerable<UserToInviteDto>> GetUserToInviteInGroup(Guid groupId, string? name);

    Task AddUsersToGroupAsync(IEnumerable<Guid> ids, Guid groupId);
    
    Task<IEnumerable<GroupMemberDto>> GetGroupMembersAsync(Guid groupId);
}