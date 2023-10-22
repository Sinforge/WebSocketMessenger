using Dapper;
using Microsoft.Extensions.Logging;
using WebSocketMessenger.Core.Interfaces.Repositories;
using WebSocketMessenger.Core.Models;

namespace WebSocketMessenger.Infrastructure.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<IMessageRepository> _logger;
        public MessageRepository(ApplicationContext context, ILogger<IMessageRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> CreateMessageAsync(Message message)
        {
            bool result = false;
            string insertQuery = "insert into public.message (\"SenderId\", \"ReceiverId\", \"Content\", \"MessageType\", \"MessageContentType\", \"SendTime\") values" +
                "(@SenderId, @ReceiverId, @Content, @MessageType, @MessageContentType, @SendTime);";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, message);
                result = true;
            }

            return result;
        }

        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            bool result = false;
            string deleteQuery = "delete from public.message where \"Id\" = @id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(deleteQuery, new { id = messageId });
                result = true;
            }

            return result;

        }

        public async Task<IEnumerable<Message>> GetConversationMessagesAsync(Guid userId1, Guid userId2)
        {
            IEnumerable<Message> result = new LinkedList<Message>();
            string selectQuery = "select * from public.message where " +
                "(\"ReceiverId\" = @userId1 and \"SenderId\" = @userId2) or" +
                "(\"ReceiverId\" = @userId2 and \"SenderId\" = @userId1); ";

            using (var connection = _context.CreateConnection())
            {
                result = await connection.QueryAsync<Message>(selectQuery, new { userId1 = userId1, userId2 = userId2 });
            }

            return result;
        }

        public async Task<IEnumerable<Message>> GetGroupMessagesAsync(Guid groupId)
        {
            IEnumerable<Message> result = new LinkedList<Message>();
            string selectQuery = "select * from public.message where " +
                "\"ReceiverId\" = @groupId;";

            using (var connection = _context.CreateConnection())
            {
                result = await connection.QueryAsync<Message>(selectQuery, new { groupId = groupId });
            }

            return result;
        }

        public async Task<Message?> GetMessageByIdAsync(int messageId)
        {
            Message result = null;
            string selectQuery = "select * from public.message where \"Id\" = @id";

            using (var connection = _context.CreateConnection())
            {
                result = await connection.QuerySingleOrDefaultAsync<Message>(selectQuery, new { id = messageId });
            }

            return result;
        }

        public async Task<bool> UpdateMessageAsync(int messageId, string content, int contentType)
        {
            bool result = false;
            string updateQuery = "update public.message set " +
                "\"Content\" = @content, \"MessageContentType\" = @type where \"Id\" = @id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(updateQuery, new { content = content, type = contentType, id = messageId });
                result = true;
            }

            return result;
        }
    }
}
