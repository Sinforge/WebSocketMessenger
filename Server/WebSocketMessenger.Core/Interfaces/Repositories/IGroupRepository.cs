using WebSocketMessenger.Core.Dtos;
using WebSocketMessenger.Core.DTOs;

namespace WebSocketMessenger.Core.Interfaces.Repositories
{
    public interface IGroupRepository
    {
        public Task<Guid> CreateGroupAsync(string name, Guid creatorId);
        public Task<bool> UpdateGroupNameAsync(Guid groupId, string groupName);
        public Task<bool> KickUserFromGroupAsync(Guid groupId,Guid userId);
        public Task<bool> UpdateUserGroupRoleAsync(Guid groupId, Guid userId, int roleId);
        public Task<bool> AddUserToGroupAsync(Guid groupId, Guid userId);
        public Task<IEnumerable<Guid>> GetUserIdsByGroupAsync(Guid groupId);

        public Task<bool> IsGroupMember(Guid userId, Guid groupId);

        public Task<IEnumerable<(Guid id, string name)>> GetUserGroupsAsync(Guid userId);

        public Task<IEnumerable<Guid>> GetGroupMembersListAsync(Guid groupId);

        public Task AddUsersToGroupAsync(IEnumerable<Guid> ids, Guid groupId);

        public Task<IEnumerable<GroupMemberDto>> GetGroupMembersAsync(Guid groupId);
        
    }
}
