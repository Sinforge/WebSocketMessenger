using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSockerMessenger.Core.DTOs.WebSocket.Base
{
    public class MessageContentBase
    {
        public virtual Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, IMessageRepository messageRepository) {
            return Task.CompletedTask;
        }
    }
}
