using Newtonsoft.Json;
using System.Text;
using WebSocketMessenger.Core.Interfaces.WS;
using WebSocketMessenger.Core.Models;
using WebSocketMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Conversation.Text

{
    public class CreateMessageContent : MessageContentBase
    {
        public string Content { get; set; }

        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            Message message = new Message
            {
                ReceiverId = header.To,
                SenderId = header.From,
                SendTime = header.SendTime,
                Content = Content,
                MessageContentType = header.Content,
                MessageType = header.Type
            };
            _ =  repositoryCollection.MessageRepository.CreateMessageAsync(message);
            _ =  connectionManager.NotifySocketsAsync(header.To.ToString(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)), header.Type);
        }
    }
}
