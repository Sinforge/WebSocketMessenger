using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebSocketMessenger.Core.Interfaces.WS;
using WebSocketMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Conversation
{
    public class DeleteMessageContent : MessageContentBase
    {
        public int MessageId { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            _ = await repositoryCollection.MessageRepository.DeleteMessageAsync(MessageId);
            _ =  connectionManager.NotifySocketsAsync(header.To.ToString(), Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                MessageType = "DeleteMessage",
                MessageId = MessageId
            })), header.Type);
           
        }

    }
}
