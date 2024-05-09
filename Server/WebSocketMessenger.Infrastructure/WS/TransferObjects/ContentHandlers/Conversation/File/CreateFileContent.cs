using System.Text;
using System.Text.Json;
using WebSocketMessenger.Core.Interfaces.WS;
using WebSocketMessenger.Core.Models;
using WebSocketMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Conversation.File

{
    public class CreateFileContent : MessageContentBase
    {
        
        public string OriginalName { get; set; }
        // FileName in system
        public string FileName { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            Message message = new Message
            {
                ReceiverId = header.To,
                SenderId = header.From,
                SendTime = header.SendTime,
                Content = FileName,
                MessageContentType = header.Content,
                MessageType = header.Type
            };
            var id = await  repositoryCollection.MessageRepository.CreateMessageAsync(message);
            var username = await repositoryCollection.UserRepository.GetUsernameByIdAsync(header.From);

            var separatorIndex = FileName.IndexOf('_');
            var fileName = FileName.Substring(separatorIndex + 1, FileName.Length - 1 - separatorIndex);
            
            
            await connectionManager.NotifySocketsAsync(header.From.ToString(), Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                Id = id,
                ReceiverId = header.To,
                SenderId = header.From,
                SendTime = header.SendTime,
                Content = fileName,
                messageContentType = header.Content,
                MessageType = "CreatedMessage",
                Username = username
            })), header.Type);
            await connectionManager.NotifySocketsAsync(header.To.ToString(), Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                Id = id,
                ReceiverId = header.To,
                SenderId = header.From,
                SendTime = header.SendTime,
                Content = fileName,
                messageContentType = header.Content,
                MessageType = "CreateMessage",
                Username = username
            })), header.Type);
        }
        

    }
}
