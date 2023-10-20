using System.Net.WebSockets;
using System.Text;
using WebSockerMessenger.Core.Interfaces.WS;
using WebSockerMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Conversation
{
    public class DeleteMessageContent : MessageContentBase
    {
        public int MessageId { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            _ =  repositoryCollection.MessageRepository.DeleteMessageAsync(MessageId);
            _ =  connectionManager.NotifySocketsAsync(header.To.ToString(), Encoding.UTF8.GetBytes(MessageId.ToString()), header.Type);
           
        }

    }
}
