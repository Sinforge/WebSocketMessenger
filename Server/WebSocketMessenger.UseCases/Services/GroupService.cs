using WebSocketMessenger.Core.Dtos;
using WebSocketMessenger.Core.DTOs;
using WebSocketMessenger.Core.Interfaces.Repositories;
using WebSocketMessenger.Core.Interfaces.Services;
using WebSocketMessenger.Core.Models;

namespace WebSocketMessenger.UseCases.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    public GroupService(IGroupRepository groupRepository, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }

    public async Task<Guid> CreateGroupAsync(Guid userId, string groupName)
    {
        return await _groupRepository.CreateGroupAsync(groupName, userId);
    }

    public async Task<IEnumerable<UserToInviteDto>> GetUserToInviteInGroup(Guid groupId, string? name)
    {
        var userIds = (await _groupRepository.GetUserIdsByGroupAsync(groupId)).ToArray();
        IEnumerable<User> users = new List<User>();
        if (userIds.Any())
        {
            if (name is not null)
            {
                users = await _userRepository.GetUsersByIdsAndSearchStringAsync(userIds, name, false);
            }
            else
            {
                users = await _userRepository.GetUsersByIdsAsync(userIds, false);

            }
        }

        return users.Select(x => new UserToInviteDto()
        {
            Id = x.Id,
            FirstName = x.Name,
            SecondName = x.Surname,
            Username = x.UserName,
        });
    }

    public async Task AddUsersToGroupAsync(IEnumerable<Guid> ids, Guid groupId)
    {
        await _groupRepository.AddUsersToGroupAsync(ids, groupId);
    }

    public async Task<IEnumerable<GroupMemberDto>> GetGroupMembersAsync(Guid groupId)
    {
        return await _groupRepository.GetGroupMembersAsync(groupId);
    }

    public async Task KickUserFromGroupAsync(Guid groupId, Guid userId)
    { 
        await _groupRepository.KickUserFromGroupAsync(groupId, userId);
    }

    public async Task UpdateUserGroupRoleAsync(Guid groupId, Guid userId, int roleId)
    {
        await _groupRepository.UpdateUserGroupRoleAsync(groupId, userId, roleId);
    }
}