using WebSocketMessenger.Core.Interfaces.Repositories;
using WebSocketMessenger.Core.Interfaces.Services;

namespace WebSocketMessenger.UseCases.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;

    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<Guid> CreateGroupAsync(Guid userId, string groupName)
    {
        return await _groupRepository.CreateGroupAsync(groupName, userId);
    }
}