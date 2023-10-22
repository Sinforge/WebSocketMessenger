using Dapper;
using Microsoft.Extensions.Logging;
using WebSocketMessenger.Core.Interfaces.Repositories;

namespace WebSocketMessenger.Infrastructure.Data.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<IGroupRepository> _logger;
        public GroupRepository(ApplicationContext context, ILogger<IGroupRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> AddUserToGroupAsync(Guid groupId, Guid userId)
        {
            bool result = false;
            string insertUserToGroup = "insert into public.user_group (group_id, user_id, role_id) values (@group_id, @user_id, @role_id);";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(insertUserToGroup, new { group_id = groupId, user_id = userId, role_id = 3 });
                result = true;
            }

            return result;
        }

        public async Task<bool> CreateGroupAsync(string name, Guid creatorId)
        {
            bool result = false;
            Guid groupId = Guid.NewGuid();
            string insertNewGroupQuery = "insert into public.group (id, name) values (@groupId, @name);";
            string insertUserGroupRelationQuery = "insert into public.user_group (group_id, user_id, role_id) values (@group_id, @user_id, @role_id);";

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(insertNewGroupQuery, new { groupId = groupId, name = name });
                    await connection.ExecuteAsync(insertUserGroupRelationQuery, new { group_id = groupId, user_id = creatorId, role_id = 1 });
                    result = true;
                    transaction.Commit();
                }

            }


            return result;

        }

        public async Task<IEnumerable<Guid>> GetUserIdsByGroupAsync(Guid groupId)
        {
            IEnumerable<Guid> result = null;
            string selectQuery = "select user_id from public.user_group where group_id = @group_id";
            using (var connection = _context.CreateConnection())
            {
                result = await connection.QueryAsync<Guid>(selectQuery, new { group_id = groupId });
            }


            return result;
        }

        public async Task<bool> IsGroupMember(Guid userId, Guid groupId)
        {
            Guid id = Guid.Empty;
            string selectQuery = "select group_id from public.user_group where group_id = @group_id and user_id = @user_id;";

            using (var connection = _context.CreateConnection())
            {
                id = await connection.QueryFirstOrDefaultAsync<Guid>(selectQuery, new { group_id = groupId, user_id = userId });
            }
            return id != Guid.Empty;

        }

        public async Task<bool> KickUserFromGroupAsync(Guid groupId, Guid userId)
        {
            bool result = false;
            string deleteQuery = "delete from public.user_group where group_id = @group_id and user_id = @user_id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(deleteQuery, new { group_id = groupId, user_id = userId });
                result = true;
            }

            return result;
        }

        public async Task<bool> UpdateGroupNameAsync(Guid groupId, string groupName)
        {
            bool result = false;
            string updateQuery = "update public.group set name = @name where id= @group_id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(updateQuery, new { name = groupName, group_id = groupId });
                result = true;
            }
            return result;
        }

        public async Task<bool> UpdateUserGroupRoleAsync(Guid groupId, Guid userId, int roleId)
        {
            bool result = false;
            string updateQuery = "update public.user_group set role_id = @role_id where group_id = @group_id and user_id = @user_id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(updateQuery, new { role_Id = roleId, group_id = groupId, user_id = userId });
                result = true;
            }

            return result;
        }
    }
}
