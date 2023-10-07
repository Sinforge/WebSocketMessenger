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
    public class UpdateUserGroupRoleContent : MessageContentBase
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            await repositoryCollection.GroupRepository.UpdateUserGroupRoleAsync(GroupId, UserId, RoleId);
            var webSockets = connectionManager.GetAllSockets()[UserId.ToString()];
 
        }
    }
}
