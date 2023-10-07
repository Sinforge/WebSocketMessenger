using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.DTOs.WebSocket.Base;
using WebSockerMessenger.Core.Models;
using WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions;
using WebSocketMessenger.Infrastructure.WS.Objects;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSocketMessenger.Infrastructure.WS.Objects.Content.Conversation
{
    public class DeleteMessageContent : MessageContentBase
    {
        // JSON

        //// Create message:
        //"data": {
        //    "From" : "user_id",
        //    "Content": "content",
        //    "To": "user_id";
        //}

        //// Update message:
        //"data": {
        //    "MessageId" : "message_id",
        //    "Content": "content"
        //}

        //// Delete message:
        //"data": {
        //    "MessageId": "message_id"
        //}

        //{
        //    "Type" : "Group/Private"
        //    "Method": "Create/Delete/Update",
        //    "Content": "Text/File",
        //    "Message": {
        //        "data": "..."
        //    }
        //}
        public int MessageId { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            await repositoryCollection.MessageRepository.DeleteMessageAsync(MessageId);
            var webSockets = connectionManager.GetAllSockets()[header.To.ToString()];
            foreach (var webSocket in webSockets)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(
                    Encoding.UTF8.GetBytes("Message deleted")),
                    WebSocketMessageType.Text,
                    true,
                    cancellationToken: CancellationToken.None
                );
            }
        }
        //public override void Handle()
        //{
        //    base.Handle();
        //}
    }
}
