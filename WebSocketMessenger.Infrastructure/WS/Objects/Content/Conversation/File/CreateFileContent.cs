﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using WebSockerMessenger.Core.DTOs.WebSocket.Base;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;
using WebSockerMessenger.Core.Models;
using WebSockerMessenger.Core.Utils;

namespace WebSocketMessenger.Infrastructure.WS.Objects.Content.Conversation.File

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
            var webSockets = connectionManager.GetAllSockets()[header.To.ToString()];
            foreach (var webSocket in webSockets)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this))),
                    WebSocketMessageType.Text,
                    true,
                    cancellationToken: CancellationToken.None
                );
            }
        }
    }
}
