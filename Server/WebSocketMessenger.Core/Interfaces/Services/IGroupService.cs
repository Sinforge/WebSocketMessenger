namespace WebSocketMessenger.Core.Interfaces.Services;

public interface IGroupService
{
    Task<Guid> CreateGroupAsync(Guid userId, string groupName);
}