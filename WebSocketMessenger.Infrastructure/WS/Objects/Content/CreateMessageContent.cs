using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.DTOs.WebSocket.Base;
using WebSockerMessenger.Core.Models;
using WebSocketMessenger.Infrastructure.Data.Repositories;
using WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSockerMessenger.Core.DTOs.WebSocket.Content
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
    public class CreateMessageContent : MessageContentBase
    {
        public string Content { get; set; }

        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, IMessageRepository messageRepository)
        {
            Message message = new Message
            {
                ReceiverId = header.To,
                SenderId = header.From,
                SendTime = header.SendTime,
                Content = this.Content,
                MessageContentType = header.Content,
                MessageType = header.Type
            };
            await messageRepository.CreateMessageAsync(message);
            var webSockets = connectionManager.GetAllSockets()[header.To.ToString()];
            foreach (var webSocket in webSockets)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))),
                    WebSocketMessageType.Text,
                    true,
                    cancellationToken: CancellationToken.None
                );
            }
        }
    }
}
