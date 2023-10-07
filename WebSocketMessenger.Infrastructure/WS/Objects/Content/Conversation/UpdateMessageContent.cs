using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.DTOs.WebSocket.Base;
using WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions;
using WebSocketMessenger.Infrastructure.WS.Objects;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSocketMessenger.Infrastructure.WS.Objects.Content.Conversation
{
    public class UpdateMessageContent : MessageContentBase
    {
        public int MessageId { get; set; }
        public string NewContent { get; set; }

        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            await repositoryCollection.MessageRepository.UpdateMessageAsync(MessageId, NewContent);
            var webSockets = connectionManager.GetAllSockets()[header.To.ToString()];
            foreach (var webSocket in webSockets)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(
                    Encoding.UTF8.GetBytes("Message updated")),
                    WebSocketMessageType.Text,
                    true,
                    cancellationToken: CancellationToken.None
                );
            }
        }
    }
}
