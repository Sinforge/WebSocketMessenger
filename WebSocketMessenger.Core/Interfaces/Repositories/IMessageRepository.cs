using WebSocketMessenger.Core.Models;
namespace WebSocketMessenger.Core.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task<bool> CreateMessageAsync(Message message);
        Task<bool> UpdateMessageAsync(int messageId, string Content, int contentType);
        Task<bool> DeleteMessageAsync(int messageId);
        Task<Message?> GetMessageByIdAsync(int messageId);
        Task<IEnumerable<Message>> GetConversationMessagesAsync(Guid userId1, Guid userid2);
        Task<IEnumerable<Message>> GetGroupMessagesAsync(Guid groupId);
    }
}
