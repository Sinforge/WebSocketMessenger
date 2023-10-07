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
    public class KickUserContent : MessageContentBase
    {
        public Guid GroupId { get; set; }
        public Guid KickedUser { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            await repositoryCollection.GroupRepository.KickUserFromGroupAsync(GroupId, KickedUser);
            var webSockets = connectionManager.GetGroupSockets(await repositoryCollection.GroupRepository.GetUserIdsByGroupAsync(GroupId));
            foreach (var webSocket in webSockets)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(
                    Encoding.UTF8.GetBytes("User kicked updated")),
                    WebSocketMessageType.Text,
                    true,
                    cancellationToken: CancellationToken.None
                );
            }
        }
    }
}
