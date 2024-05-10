using WebSocketMessenger.Core.Dtos;
using WebSocketMessenger.Core.Models;
namespace WebSocketMessenger.Core.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task<int> CreateMessageAsync(Message message);
        Task<bool> UpdateMessageAsync(int messageId, string Content, int contentType);
        Task<bool> DeleteMessageAsync(int messageId);
        Task<Message?> GetMessageByIdAsync(int messageId);
        Task<IEnumerable<Message>> GetConversationMessagesAsync(Guid userId1, Guid userid2);
        Task<IEnumerable<Message>> GetGroupMessagesAsync(Guid groupId);

        Task<IEnumerable<DialogItemDto>> GetUserDialogs(Guid userId);

        Task<Guid> GetMessageOwner(int messageId);

        Task<Guid> GetMessageReceiver(int messageId);

        Task<IEnumerable<(Guid id, string message, DateTime sendTime)>> GetGroupsLastMessagesAsync(IEnumerable<Guid> groupIds);
    }
}
