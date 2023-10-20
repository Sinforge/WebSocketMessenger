using System.Net.WebSockets;
using System.Text;
using WebSockerMessenger.Core.Interfaces.WS;
using WebSockerMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Group
{
    public class AddUserToGroupContent : MessageContentBase
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            await repositoryCollection.GroupRepository.AddUserToGroupAsync(GroupId, UserId);
            await connectionManager.NotifySocketsAsync(header.To.ToString(), Encoding.UTF8.GetBytes($"You was added in group {GroupId}"), header.Type);
           
        }
    }
}
