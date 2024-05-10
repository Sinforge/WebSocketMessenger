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

        public async Task<IEnumerable<DialogItemDto>> GetUserDialogs(Guid userId)
        {
            IEnumerable<DialogItemDto> result = null;
            try
            {
                result = new LinkedList<DialogItemDto>();
                string selectQuery = "WITH RankedMessages AS (SELECT  \"LastMessage\",\r\n        \"UserName\",\r\n        \"UserId\",\r\n        ROW_NUMBER() OVER (PARTITION BY \"UserId\" ORDER BY \"LastMessage\" DESC) AS rn\r\n    FROM (\r\n        select MAX(m.\"SendTime\") as \"LastMessage\",\r\n            case\r\n                when m.\"ReceiverId\" = @id then u_sender.username\r\n                else u_receiver.username\r\n            end as \"UserName\",\r\n            case\r\n                when m.\"ReceiverId\" = @id then u_sender.id\r\n                else u_receiver.id\r\n            end as \"UserId\"\r\n        from public.message m\r\n        join public.user u_sender on m.\"SenderId\" = u_sender.id\r\n        join public.user u_receiver on m.\"ReceiverId\" = u_receiver.id\r\n        where (m.\"SenderId\" = @id or m.\"ReceiverId\" = @id)\r\n  " +
                    "          and m.\"MessageType\" = 1\r\n        group by m.\"SenderId\", m.\"ReceiverId\", u_sender.username, u_receiver.username, u_sender.id, u_receiver.id\r\n    ) AS subquery\r\n)\r\nSELECT \"LastMessage\", \"UserName\", \"UserId\" as \"Id\"\r\nFROM RankedMessages\r\nWHERE rn = 1";

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
            groupIds.ToList().ForEach(x => Console.WriteLine(x));
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
