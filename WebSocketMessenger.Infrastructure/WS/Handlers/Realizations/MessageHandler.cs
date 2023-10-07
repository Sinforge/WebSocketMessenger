using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.DTOs.WebSocket.Base;
using WebSockerMessenger.Core.Models;
using WebSockerMessenger.Core.Utils;
using WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions;
using WebSocketMessenger.Infrastructure.WS.Handlers.Abstractions;
using WebSocketMessenger.Infrastructure.WS.Objects;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSocketMessenger.Infrastructure.WS.Handlers.Realizations
{
    public class MessageHandler : IWebSocketMessageHandler
    {
        private readonly IWebSocketConnectionManager _connectionManager;
        private readonly MessageFactory _messageFactory;
        private readonly RepositoryCollection _repositoryCollection;
        public MessageHandler(IWebSocketConnectionManager connectionManager, MessageFactory messageFactory, RepositoryCollection repositoryCollection)
        {
            _repositoryCollection = repositoryCollection;
            _messageFactory = messageFactory;
            _connectionManager = connectionManager;
        }


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
        //    "Type" : "Group/User"
        //    "Method": "Create/Delete/Update",
        //    "Content": "Text/File",
        //    "Message": {
        //        "data": "..."
        //    }
        //}
        public async Task HandleMessage(WebSocketReceiveResult result, byte[] message)
        {
            string messageString;
            if (result.MessageType == WebSocketMessageType.Text) {
                messageString = Encoding.UTF8.GetString(message, 0, result.Count);
                IMessageBase<MessageContentBase>? castedToMessageType = AnalizeDynamic(messageString);
                await castedToMessageType.Handle(_connectionManager, _repositoryCollection);
   
                
            }

            

            
        }
        private  IMessageBase<MessageContentBase>? AnalizeDynamic(string json)
        {

            // Получение текущей сборки
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Получение всех типов из текущей сборки
            IEnumerable<Type> types = assembly.GetTypes().Where(t => t.BaseType == typeof(MessageContentBase));

            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);

            // Проходимся по каждому типу
            foreach (Type type in types)
            {
                try
                {
                    if(type.Name.Contains((string) obj.HeaderInfo.Method))
                    {
                        var newType = typeof(MessageBase<>).MakeGenericType(type);
                        return ((IMessageBase<MessageContentBase>?)JsonConvert.DeserializeObject(json, newType));
                    }
                    
                }
                catch (JsonException ex)
                {
                    continue;
                }



            }
            return null;


        }

    }
}
