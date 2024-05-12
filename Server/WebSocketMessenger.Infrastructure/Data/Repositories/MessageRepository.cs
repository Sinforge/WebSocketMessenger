using Dapper;
using Microsoft.Extensions.Logging;
using WebSocketMessenger.Core.Dtos;
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

        public async Task<int> CreateMessageAsync(Message message)
        {
            int result = -1;
            string insertQuery = "insert into public.message (\"SenderId\", \"ReceiverId\", \"Content\", \"MessageType\", \"MessageContentType\", \"SendTime\") values" +
                "(@SenderId, @ReceiverId, @Content, @MessageType, @MessageContentType, @SendTime) returning \"Id\";";

            using var connection = _context.CreateConnection();
            result = await connection.ExecuteScalarAsync<int>(insertQuery, message);

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
            string selectQuery = "SELECT * FROM public.message \nWHERE (\"ReceiverId\" = @userId1 AND \"SenderId\" = @userId2) \n    OR (\"ReceiverId\" = @userId2 AND \"SenderId\" = @userId1)\nORDER BY \"SendTime\"";

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
                "\"ReceiverId\" = @groupId order by \"SendTime\";";

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

        public async Task<IEnumerable<DialogItemDto>> GetUserDialogs(Guid userId)
        {
            IEnumerable<DialogItemDto> result = null;
            try
            {
                result = new LinkedList<DialogItemDto>();
                string selectQuery =
                    "WITH UserDialogs AS (\n    SELECT DISTINCT \"ReceiverId\" AS \"UserId\"\n    FROM message\n    WHERE \"SenderId\" = @id AND \"MessageType\" = 1\n\n    UNION\n\n    SELECT DISTINCT \"SenderId\" AS \"UserId\"\n    FROM message\n    WHERE \"ReceiverId\" = @id AND \"MessageType\" = 1\n)\n\nSELECT \n    ud.\"UserId\" as \"Id\",\n    concat(u.name, ' ', u.surname, ' (', u.username, ')') AS \"Username\",\n    CASE \n        WHEN m.\"MessageContentType\" = 2 THEN substring(m.\"Content\" from '^[^_]+_(.*)$') \n        ELSE m.\"Content\" \n    END as \"LastMessage\",\n    m.\"SendTime\" as \"SendTime\"\nFROM \n    UserDialogs ud\nJOIN (\n    SELECT \n        CASE \n            WHEN \"SenderId\" = @id THEN \"ReceiverId\"\n            ELSE \"SenderId\"\n        END AS \"OtherUserId\",\n        MAX(\"SendTime\") AS \"MaxSendTime\"\n    FROM \n        message\n    WHERE \n        (\"SenderId\" = @id OR \"ReceiverId\" = @id) AND \"MessageType\" = 1\n    GROUP BY \n        CASE \n            WHEN \"SenderId\" = @id THEN \"ReceiverId\"\n            ELSE \"SenderId\"\n        END\n) latest_message ON ud.\"UserId\" = latest_message.\"OtherUserId\"\nJOIN message m ON (m.\"SenderId\" = ud.\"UserId\" OR m.\"ReceiverId\" = ud.\"UserId\")\n    AND m.\"SendTime\" = latest_message.\"MaxSendTime\"\nJOIN public.user u ON u.id = latest_message.\"OtherUserId\";\n";

                using (var connection = _context.CreateConnection())
                {
                    result = await connection.QueryAsync<DialogItemDto>(selectQuery, new { id = userId });
                }
            }
            catch(Exception ex)
            {

            }
            return result;

        }

        public async Task<Guid> GetMessageOwner(int messageId)
        {
            var selectQuery = "select \"SenderId\" from public.message where id = @id";
            var result = Guid.Empty;
            using (var connection = _context.CreateConnection())
            {
                result = await connection.QuerySingleOrDefaultAsync<Guid>(selectQuery, new { id = messageId });
            }

            return result;
        }

        public async Task<Guid> GetMessageReceiver(int messageId)
        {
            var selectQuery = "select \"ReceiverId\" from public.message where id = @id";
            var result = Guid.Empty;
            using (var connection = _context.CreateConnection())
            {
                result = await connection.QuerySingleOrDefaultAsync<Guid>(selectQuery, new { id = messageId });
            }

            return result;        }

        public async Task<IEnumerable<(Guid id, string message, DateTime sendTime)>> GetGroupsLastMessagesAsync(IEnumerable<Guid> groupIds)
        {
            var selectQuery = "select \"ReceiverId\" as id, \"Content\" as message, \"SendTime\" as \"sendTime\" " +
                              " from (select m.*, ROW_NUMBER() OVER (PARTITION BY \"ReceiverId\" ORDER BY \"SendTime\" DESC) " +
                              " AS rn FROM message m where \"ReceiverId\" = ANY(@ids) and \"MessageType\" = 2) as result where rn = 1";

            var ids = groupIds.ToArray();
            using var connection = _context.CreateConnection();
            var result = 
                await connection.QueryAsync<(Guid id, string message, DateTime sendTime )>(selectQuery, new { ids}) ?? 
                new List<(Guid id, string message, DateTime sendTime)>();

            return result ?? new List<(Guid id, string message, DateTime sendTime)>();  
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
