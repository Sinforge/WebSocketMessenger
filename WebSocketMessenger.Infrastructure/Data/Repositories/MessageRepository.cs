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
            string insertQuery = "insert into public.message (\"SenderId\", \"ReceiverId\", \"Content\", \"MessageType\", \"MessageContentType\", \"SendTime\") values" +
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
            string deleteQuery = "delete from public.message where \"Id\" = @id";
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

        public async Task<Message?> GetMessageByIdAsync(int messageId)
        {
            Message result = null;
            string selectQuery = "select * from public.message where \"Id\" = @id";
            try
            {
                using(var connection = _context.CreateConnection())
                {
                    result = await connection.QuerySingleOrDefaultAsync<Message>(selectQuery, new {id = messageId});
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Cant find message with id: {messageId} {e.Message}");
            }
            return result;
        }

        public async Task<bool> UpdateMessageAsync(int messageId, string content, int contentType)
        {
            bool result = false;
            string updateQuery = "update public.message set " +
                "\"Content\" = @content, \"MessageContentType\" = @type where \"Id\" = @id";
            try
            {
                using(var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(updateQuery, new { content = content, type = contentType, id = messageId });
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
