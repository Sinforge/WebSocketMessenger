using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.Models;
using WebSockerMessenger.Core.Utils;
using WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions;
using WebSocketMessenger.Infrastructure.WS.Handlers.Abstractions;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSocketMessenger.Infrastructure.WS.Handlers.Realizations
{
    public class MessageHandler : IWebSocketMessageHandler
    {
        private readonly IWebSocketConnectionManager _connectionManager;
        private readonly MessageFactory _messageFactory;
        private readonly IMessageRepository _messageRepository;
        public MessageHandler(IWebSocketConnectionManager connectionManager, MessageFactory messageFactory, IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
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
                var routeOb = JsonConvert.DeserializeObject<dynamic>(messageString);
                if(routeOb?.Method == "Create")
                {
                    Message messageObj = _messageFactory.CreateMessage(routeOb);
                    await _messageRepository.CreateMessageAsync(messageObj);
                    var webSockets = _connectionManager.GetAllSockets()[(string)(routeOb.Message.Data.To)];
                    foreach(var webSocket in webSockets)
                    {
                        await webSocket.SendAsync(new ArraySegment<byte>(message,0,result.Count), WebSocketMessageType.Text, true, cancellationToken: CancellationToken.None);
                    }
                }
                //if(routeOb?.OperationType == OperationType.Create)
                //{


                //}
                
            }

            

            
        }
        
    }
}
