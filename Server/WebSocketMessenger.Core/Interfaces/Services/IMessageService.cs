﻿using WebSocketMessenger.Core.Dtos;
using WebSocketMessenger.Core.Models;

namespace WebSocketMessenger.Core.Interfaces.Services
{
    public interface IMessageService
    {
        public Task<IEnumerable<MessageDto>> GetMessagesByUsers(Guid userId1, Guid userId2);
        public Task<MessageDTO> GetMessageByIdAsync(int messageId, Guid userId);
        public Task<IEnumerable<MessageDto>> GetMessageByGroupAsync(Guid userId, Guid groupId);
        Task<IEnumerable<DialogItemDto>> GetUserDialogs(Guid userId);


    }
}
