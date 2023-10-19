using WebSockerMessenger.Core.Models;
namespace WebSocketMessenger.Core.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task<bool> CreateMessageAsync(Message message);
        Task<bool> UpdateMessageAsync(int messageId, string Content, int contentType);
        Task<bool> DeleteMessageAsync(int messageId);
        Task<Message?> GetMessageByIdAsync(int messageId);
    }
}
