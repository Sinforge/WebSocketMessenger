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
        private readonly IUserRepository _userRepository;
        public MessageService(IMessageRepository messageRepository, IGroupRepository groupRepository, IUserRepository userRepository) {
            _messageRepository = messageRepository;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<MessageDto>> GetMessageByGroupAsync(Guid userId, Guid groupId)
        {
            if(!await _groupRepository.IsGroupMember(userId, groupId))
            {
                throw new SharedException("User not member of the group", 403);
            }

            var messages = (await _messageRepository.GetGroupMessagesAsync(groupId)).ToArray();
            var usersNames = (await _userRepository.GetUsersByIdsAsync(messages.Select(x => x.SenderId).Distinct().ToArray()))
                .ToDictionary(
                    x=> x.Id,
                    x => $"{x.Name} {x.Surname} ({x.UserName})");
            return from message in messages select 
                    new MessageDto()
                    {
                        Id = message.Id,
                        AuthorId = message.SenderId,
                        Content = message.MessageContentType == 1 
                            ? message.Content
                            : message.Content.Substring(message.Content.IndexOf('_') + 1, message.Content.Length - message.Content.IndexOf('_') - 1),
                        MessageContentType = message.MessageContentType,
                        SendTime = message.SendTime,
                        Username = usersNames[message.SenderId]
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

        public async Task<IEnumerable<DialogItemDto>> GetUserDialogs(Guid userId)
        {
            return await _messageRepository.GetUserDialogs(userId);
        }
    }
}
