﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.DTOs.WebSocket.Base;
using WebSockerMessenger.Core.Models;
using WebSockerMessenger.Core.Utils;
using WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions;
using WebSocketMessenger.Infrastructure.WS.Objects;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSocketMessenger.Infrastructure.WS.Objects.Content.Conversation.File

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

            var webSockets = connectionManager.GetAllSockets()[header.To.ToString()];
            foreach (var webSocket in webSockets)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(
                    Encoding.UTF8.GetBytes("Message updated")),
                    WebSocketMessageType.Text,
                    true,
                    cancellationToken: CancellationToken.None
                );
            }
        }
    }
}
