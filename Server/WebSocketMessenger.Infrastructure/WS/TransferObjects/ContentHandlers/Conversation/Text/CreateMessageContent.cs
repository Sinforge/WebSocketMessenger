using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
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
            var id =await repositoryCollection.MessageRepository.CreateMessageAsync(message);

            var username = await repositoryCollection.UserRepository.GetUsernameByIdAsync(header.From);
            
            _ =  connectionManager.NotifySocketsAsync(header.From.ToString(), 
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new  
                {
                    Id = id,
                    ReceiverId = header.To,
                    SenderId = header.From,
                    SendTime = header.SendTime,
                    Content = Content,
                    messageContentType = 1,
                    MessageType = "CreatedMessage",
                    Username = username
                })), header.Type);
            
            await connectionManager.NotifySocketsAsync(header.To.ToString(), 
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new  
                {
                    Id = id,
                    ReceiverId = header.To,
                    SenderId = header.From,
                    SendTime = header.SendTime,
                    Content = Content,
                    messageContentType = 1,
                    MessageType = "CreateMessage",
                    Username = username,
                })), header.Type);
        }
    }
}
