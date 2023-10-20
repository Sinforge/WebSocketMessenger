using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using WebSockerMessenger.Core.Interfaces.WS;
using WebSockerMessenger.Core.Models;
using WebSockerMessenger.Infrastructure.TransferObjets.Base;
using WebSocketMessenger.Infrastructure.FileSystem;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Conversation.File

{
    public class UpdateFileContent : MessageContentBase
    {
        public int MessageId { get; set; }
        public string FileExtention { get; set; }
        public string Content { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            Message? currentMessage = await repositoryCollection.MessageRepository.GetMessageByIdAsync(MessageId);
            if (currentMessage != null) {
                string fileName = FileManager.AddNewFile(Content, FileExtention);
                if(currentMessage.MessageContentType == 2)
                {
                    FileManager.DeleteFile(currentMessage.Content);

                }
                await repositoryCollection.MessageRepository.UpdateMessageAsync(MessageId, fileName, 2);

            }
            _ = connectionManager.NotifySocketsAsync(header.To.ToString(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(MessageId.ToString())), header.Type);
        }
    }
}
