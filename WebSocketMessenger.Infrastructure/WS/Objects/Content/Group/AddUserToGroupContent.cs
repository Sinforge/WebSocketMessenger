using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.DTOs.WebSocket.Base;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSocketMessenger.Infrastructure.WS.Objects.Content.Group
{
    public class AddUserToGroupContent : MessageContentBase
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            await repositoryCollection.GroupRepository.AddUserToGroupAsync(GroupId, UserId);
            var webSockets = connectionManager.GetAllSockets()[UserId.ToString()];
            foreach (var webSocket in webSockets)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(
                    Encoding.UTF8.GetBytes("You was added in group")),
                    WebSocketMessageType.Text,
                    true,
                    cancellationToken: CancellationToken.None
                );
            }
        }
    }
}
