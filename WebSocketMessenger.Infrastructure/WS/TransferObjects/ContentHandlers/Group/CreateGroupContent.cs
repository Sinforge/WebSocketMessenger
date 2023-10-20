using System.Net.WebSockets;
using System.Text;
using WebSockerMessenger.Core.Interfaces.WS;
using WebSockerMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Group
{
    public class CreateGroupContent : MessageContentBase
    {
        public Guid CreatorId { get; set; }
        public string Name { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            _ =  repositoryCollection.GroupRepository.CreateGroupAsync(Name, CreatorId);
            _ =  connectionManager.NotifySocketsAsync(CreatorId.ToString(), Encoding.UTF8.GetBytes("Group created"), 1);
            
        }
    }
}
