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
            string insertQuery = "insert into public.message (sender_id, receiver_id, content, message_type, message_content_type) values" +
                "(@SenderId, @ReceiverId, @Content, @MessageType, @MessageContentType);";
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
    }
}
