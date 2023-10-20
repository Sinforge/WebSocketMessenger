using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using WebSockerMessenger.Core.Interfaces.WS;
using WebSockerMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Group
{
    public class UpdateGroupContent : MessageContentBase
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            _ =  repositoryCollection.GroupRepository.UpdateGroupNameAsync(GroupId, Name);
            _ =  connectionManager.NotifySocketsAsync(GroupId.ToString(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this)), 2);
            
        }
    }
}
