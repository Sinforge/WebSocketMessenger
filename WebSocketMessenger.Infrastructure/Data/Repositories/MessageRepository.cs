using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.Models;
using WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions;

namespace WebSocketMessenger.Infrastructure.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<IMessageRepository> _logger;
        public MessageRepository(ApplicationContext context, ILogger<IMessageRepository> logger) {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> CreateMessageAsync(Message message)
        {
            bool result = false;
            string insertQuery = "insert into public.message (sender_id, receiver_id, content, message_type, message_content_type, send_date) values" +
                "(@SenderId, @ReceiverId, @Content, @MessageType, @MessageContentType, @SendTime);";
            try
            {
                using(var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(insertQuery, message);
                    result = true;
                }

            }
            catch(Exception e)
            {
                _logger.LogWarning($"Cant insert new message: {e.Message}");
            }
            return result;
        }

        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            bool result = false;
            string deleteQuery = "delete from public.message where id = @id";
            try
            {
                using(var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(deleteQuery, new { id = messageId });
                    result = true;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Cant delete message with id: {messageId} {e.Message}");
            }
            return result;

        }

        public async Task<bool> UpdateMessageAsync(int messageId, string content)
        {
            bool result = false;
            string updateQuery = "update public.message set " +
                "content = @content where id = @id";
            try
            {
                using(var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(updateQuery, new { content = content, id = messageId });
                    result = true;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Cant update message with id: {messageId} {e.Message}");
            }

            return result;
        }
    }
}
