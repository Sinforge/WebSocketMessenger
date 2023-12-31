﻿using System.Text;
using System.Text.Json;
using WebSocketMessenger.Core.Interfaces.WS;
using WebSocketMessenger.Core.Models;
using WebSocketMessenger.Infrastructure.FileSystem;
using WebSocketMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Conversation.File

{
    public class CreateFileContent : MessageContentBase
    {

        public string FileExtention { get; set; }
        public string Content { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            string fileName = FileManager.AddNewFile(Content, FileExtention);
            string copyContent = Content;
            Message message = new Message
            {
                ReceiverId = header.To,
                SenderId = header.From,
                SendTime = header.SendTime,
                Content = fileName,
                MessageContentType = header.Content,
                MessageType = header.Type
            };
            _ = repositoryCollection.MessageRepository.CreateMessageAsync(message);
            message.Content = copyContent;
            _ = connectionManager.NotifySocketsAsync(header.To.ToString(), Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)), header.Type);
        }
    }
}
