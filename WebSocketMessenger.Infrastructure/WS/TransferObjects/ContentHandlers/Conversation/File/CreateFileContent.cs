using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using WebSockerMessenger.Core.Interfaces.WS;
using WebSockerMessenger.Core.Models;
using WebSockerMessenger.Infrastructure.TransferObjets.Base;
using WebSocketMessenger.Infrastructure.FileSystem;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Conversation.File

{
    public class CreateFileContent : MessageContentBase
    {

        public string FileExtention { get; set; }
        public string Content { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            string fileName = FileManager.AddNewFile(Content, FileExtention);
            Message message = new Message
            {
                ReceiverId = header.To,
                SenderId = header.From,
                SendTime = header.SendTime,
                Content = fileName,
                MessageContentType = header.Content,
                MessageType = header.Type
            };
            await repositoryCollection.MessageRepository.CreateMessageAsync(message);
            await connectionManager.NotifySocketsAsync(header.To.ToString(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)), header.Type);
        }
    }
}
