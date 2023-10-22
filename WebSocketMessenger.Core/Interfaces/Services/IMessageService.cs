namespace WebSocketMessenger.Core.Interfaces.Services
{
    public interface IMessageService
    {
        public Task<IEnumerable<int>> GetMessagesByUsers(Guid userId1, Guid userId2);
        public Task<string> GetMessageByIdAsync(int messageId, Guid userId);
        public Task<IEnumerable<int>> GetMessageByGroupAsync(Guid userId, Guid groupId);


    }
}
