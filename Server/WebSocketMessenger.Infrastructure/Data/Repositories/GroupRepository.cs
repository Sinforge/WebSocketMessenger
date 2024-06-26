﻿using Dapper;
using Microsoft.Extensions.Logging;
using WebSocketMessenger.Core.Dtos;
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

        public async Task<Guid> CreateGroupAsync(string name, Guid creatorId)
        {
            Guid groupId = Guid.NewGuid();
            string insertNewGroupQuery = "insert into public.group (id, name) values (@groupId, @name);";
            string insertUserGroupRelationQuery = "insert into public.user_group (group_id, user_id, role_id) values (@group_id, @user_id, @role_id);";

            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            await connection.ExecuteAsync(insertNewGroupQuery, new { groupId,  name });
            await connection.ExecuteAsync(insertUserGroupRelationQuery, new { group_id = groupId, user_id = creatorId, role_id = 1 });
            transaction.Commit();
            

            return groupId;

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

        public async Task<IEnumerable<(Guid id, string name)>> GetUserGroupsAsync(Guid userId)
        {
            string selectQuery =
                "select group_id as id, name as name from public.user_group join public.group gr on group_id = gr.id where user_id = @userId;";
            IEnumerable<(Guid id, string name)> groups = new List<(Guid id, string name)>();
            using var connection = _context.CreateConnection();
            groups = await connection.QueryAsync<(Guid id, string name)>(selectQuery, new { userId });

            return groups;
        }

        public async Task<IEnumerable<Guid>> GetGroupMembersListAsync(Guid groupId)
        {
            string selectQuery =
                "select user_id from user_group where group_id = @groupId";
            using var connection = _context.CreateConnection();
            var groupUserIds = await connection.QueryAsync<Guid>(selectQuery, new { groupId });

            return groupUserIds;
        }

        public async Task AddUsersToGroupAsync(IEnumerable<Guid> ids, Guid groupId)
        {
            var newUserGroupEntities = ids.Select(x => new { group_id = groupId, user_id = x, role_id = 3 }).ToList();
            var insertQuery = "insert into public.user_group (group_id, user_id, role_id) values (@group_id, @user_id, @role_id)";
            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            await connection.ExecuteAsync(insertQuery, newUserGroupEntities, transaction);
            transaction.Commit();
        }

        public async Task<IEnumerable<GroupMemberDto>> GetGroupMembersAsync(Guid groupId)
        {
            var selectQuery =
                "select ug.user_id as \"Id\", u.username as \"Username\", u.name as \"FirstName\", u.surname as \"SecondName\", gr.name as \"RoleName\", gr.id as \"RoleId\"  " +
                "from public.user_group ug " +
                "left join public.group_role gr on ug.role_id = gr.id " +
                "join public.user u on u.id = ug.user_id " +
                "where ug.group_id = @groupId";
            using var connection = _context.CreateConnection();
            var members = await connection.QueryAsync<GroupMemberDto>(selectQuery, new { groupId });

            return members;

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
