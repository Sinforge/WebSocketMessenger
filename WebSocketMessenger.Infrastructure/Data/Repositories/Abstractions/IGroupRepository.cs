using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions
{
    public interface IGroupRepository
    {
        public Task<bool> CreateGroupAsync(string name, Guid creatorId);
        public Task<bool> UpdateGroupNameAsync(Guid groupId, string groupName);
        public Task<bool> KickUserFromGroupAsync(Guid groupId,Guid userId);
        public Task<bool> UpdateUserGroupRoleAsync(Guid grouId, Guid userId, int roleId);
        public Task<bool> AddUserToGroupAsync(Guid groupId, Guid userId);
        public Task<IEnumerable<Guid>> GetUserIdsByGroupAsync(Guid groupId);
    }
}
