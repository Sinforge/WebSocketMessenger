using WebSockerMessenger.Core.Interfaces.Services;
using WebSockerMessenger.Core.Models;
using WebSocketMessenger.Core.Interfaces.Repositories;

namespace WebSocketMessenger.UseCases.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IGroupRepository _groupRepository;
        public MessageService(IMessageRepository messageRepository, IGroupRepository groupRepository) {
            _messageRepository = messageRepository;
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<int>> GetMessageByGroupAsync(Guid userId, Guid groupId)
        {
            if(!await _groupRepository.IsGroupMember(userId, groupId))
            {
                throw new Exception("User not member of the group");
            }
            return from message in await _messageRepository.GetGroupMessagesAsync(groupId) select message.Id;
        }

        public async Task<string> GetMessageByIdAsync(int messageId, Guid userId)
        {
            Message? message = await _messageRepository.GetMessageByIdAsync(messageId);
            if(message == null)
            {
                throw new Exception("Message not found");
            }
            else
            {
                if(message.SenderId == userId || message.ReceiverId == userId) {
                    return message.Content;
                }
                else
                {
                    throw new Exception("Not access for this message");
                }
            }
        }

        public async Task<IEnumerable<int>> GetMessagesByUsers(Guid userId1, Guid userId2)
        {
            return from message in await _messageRepository.GetConversationMessagesAsync(userId1, userId2) select message.Id;

        }
    }
}
