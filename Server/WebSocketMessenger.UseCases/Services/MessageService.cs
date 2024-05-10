using WebSocketMessenger.Core.Dtos;
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

        public async Task<IEnumerable<MessageDto>> GetMessageByGroupAsync(Guid userId, Guid groupId)
        {
            if(!await _groupRepository.IsGroupMember(userId, groupId))
            {
                throw new SharedException("User not member of the group", 403);
            }
            return from message in await _messageRepository.GetGroupMessagesAsync(groupId) select 
                    new MessageDto()
                    {
                        Id = message.Id,
                        AuthorId = message.SenderId,
                        Content = message.Content,
                        MessageContentType = message.MessageContentType,
                        SendTime = message.SendTime,
                        Username = "some user"
                    };
        }

        public async Task<MessageDTO> GetMessageByIdAsync(int messageId, Guid userId)
        {
            Message? message = await _messageRepository.GetMessageByIdAsync(messageId);
            if(message == null)
            {
                throw new SharedException("Message not found", 400);
            }
            else
            {
                MessageDTO messageDTO = new MessageDTO();
                messageDTO.Type = message.MessageContentType;
                if(message.SenderId == userId || message.ReceiverId == userId) {
                    if(message.MessageContentType == 2)
                    {
                        messageDTO.Message = message.Content;
                    }
                    else
                    {
                        messageDTO.Message = message.Content;
                    }
                    return messageDTO;
                }
                else
                {
                    throw new SharedException("Not access for this message", 403);
                }
            }
        }


            public async Task<IEnumerable<MessageDto>> GetMessagesByUsers(Guid userId1, Guid userId2)
        {
            return from message in await _messageRepository.GetConversationMessagesAsync(userId1, userId2)
                   select new MessageDto {Id = message.Id, AuthorId = message.SenderId, 
                       Content = message.MessageContentType == 1 ? message.Content : message.Content.Substring(message.Content.IndexOf('_') + 1, message.Content.Length - message.Content.IndexOf('_') - 1),
                       MessageContentType = message.MessageContentType, SendTime = message.SendTime };

        }

        public Task<IEnumerable<DialogItemDto>> GetUserDialogs(Guid userId)
        {
            return _messageRepository.GetUserDialogs(userId);
        }
    }
}
