using WebSocketMessenger.Core.Exceptions;
using WebSocketMessenger.Core.Interfaces.Repositories;
using WebSocketMessenger.Core.Interfaces.Services;
using WebSocketMessenger.Core.Models;

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
                throw new SharedException("User not member of the group", 403);
            }
            return from message in await _messageRepository.GetGroupMessagesAsync(groupId) select message.Id;
        }

        public async Task<string> GetMessageByIdAsync(int messageId, Guid userId)
        {
            Message? message = await _messageRepository.GetMessageByIdAsync(messageId);
            if(message == null)
            {
                throw new SharedException("Message not found", 400);
            }
            else
            {
                if(message.SenderId == userId || message.ReceiverId == userId) {
                    return message.Content;
                }
                else
                {
                    throw new SharedException("Not access for this message", 403);
                }
            }
        }

        public async Task<IEnumerable<int>> GetMessagesByUsers(Guid userId1, Guid userId2)
        {
            return from message in await _messageRepository.GetConversationMessagesAsync(userId1, userId2) select message.Id;

        }
    }
}
