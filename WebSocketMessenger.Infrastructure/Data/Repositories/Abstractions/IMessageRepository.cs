using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.Models;

namespace WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions
{
    public interface IMessageRepository
    {
        Task<bool> CreateMessageAsync(Message message);
        Task<bool> UpdateMessageAsync(int messageId, string Content, int contentType);
        Task<bool> DeleteMessageAsync(int messageId);
        Task<Message?> GetMessageByIdAsync(int messageId);
    }
}
