using System.Text;
using System.Text.Json;
using WebSocketMessenger.Core.Interfaces.WS;
using WebSocketMessenger.Core.Models;
using WebSocketMessenger.Infrastructure.FileSystem;
using WebSocketMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Conversation.Text
{
    public class UpdateMessageContent : MessageContentBase
    {
        public int MessageId { get; set; }
        public string NewContent { get; set; }

        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            Message? currentMessage = await repositoryCollection.MessageRepository.GetMessageByIdAsync(MessageId);
            if (currentMessage != null)
            {
                if (currentMessage.MessageContentType == 2)
                {
                    FileManager.DeleteFile(currentMessage.Content);

                }
                await repositoryCollection.MessageRepository.UpdateMessageAsync(MessageId, NewContent, 1);
                
            }
            _ = connectionManager.NotifySocketsAsync(header.To.ToString(), Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
            {
                messageType="update",
                newContent=NewContent,
                messageId = MessageId
            })), header.Type);
            
        }
    }
}
