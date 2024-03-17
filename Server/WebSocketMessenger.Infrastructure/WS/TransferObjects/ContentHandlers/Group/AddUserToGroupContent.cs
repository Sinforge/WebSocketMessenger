using System.Text;
using WebSocketMessenger.Core.Interfaces.WS;
using WebSocketMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Group
{
    public class AddUserToGroupContent : MessageContentBase
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            _ =  repositoryCollection.GroupRepository.AddUserToGroupAsync(GroupId, UserId);
            _ =  connectionManager.NotifySocketsAsync(header.To.ToString(), Encoding.UTF8.GetBytes($"You was added in group {GroupId}"), header.Type);
           
        }
    }
}
