using WebSocketMessenger.Core.Dtos;

namespace WebSocketMessenger.Core.Interfaces.Repositories
{
    public interface IGroupRepository
    {
        public Task<Guid> CreateGroupAsync(string name, Guid creatorId);
        public Task<bool> UpdateGroupNameAsync(Guid groupId, string groupName);
        public Task<bool> KickUserFromGroupAsync(Guid groupId,Guid userId);
        public Task<bool> UpdateUserGroupRoleAsync(Guid grouId, Guid userId, int roleId);
        public Task<bool> AddUserToGroupAsync(Guid groupId, Guid userId);
        public Task<IEnumerable<Guid>> GetUserIdsByGroupAsync(Guid groupId);

        public Task<bool> IsGroupMember(Guid userId, Guid groupId);

        public Task<IEnumerable<(Guid id, string name)>> GetUserGroupsAsync(Guid userId);
    }
}
