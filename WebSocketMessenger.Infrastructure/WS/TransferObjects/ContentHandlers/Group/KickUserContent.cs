using System.Net.WebSockets;
using System.Text;
using WebSockerMessenger.Core.Interfaces.WS;
using WebSockerMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Group
{
    public class KickUserContent : MessageContentBase
    {
        public Guid GroupId { get; set; }
        public Guid KickedUser { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            _ =  repositoryCollection.GroupRepository.KickUserFromGroupAsync(GroupId, KickedUser);
            _ =  connectionManager.NotifySocketsAsync(GroupId.ToString(), Encoding.UTF8.GetBytes($"User with id: {KickedUser} kicked"), 2);
                   
        }
    }
}
