using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSocketMessenger.Infrastructure.WS.Handlers.Abstractions;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSocketMessenger.Infrastructure.WS.Handlers.Realizations
{
    public class MessageHandler : IWebSocketMessageHandler
    {
        private readonly IWebSocketConnectionManager _connectionManager;
        public MessageHandler(IWebSocketConnectionManager connectionManager)
        {
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
        //    "MessageType": "Create/Delete/Update",
        //    "MessageContent": "Text/File",
        //    "Message": {
        //        "data": "..."
        //    }
        //}
        public void HandleMessage(WebSocketReceiveResult result, byte[] message)
        {
            string messageString;
            if (result.MessageType == WebSocketMessageType.Text) {
                messageString = Encoding.UTF8.GetString(message, 0, result.Count);
                var routeOb = JsonConvert.DeserializeObject<dynamic>(messageString);
                Console.WriteLine(messageString);
                //if(routeOb?.OperationType == OperationType.Create)
                //{


                //}
                
            }

            

            
        }
        private void HandleCreateMessageDynamic(dynamic message)
        {
            switch(message.MessageType)
            {
                case "Create":
                    if(message.MessageContent == "Text")
                    {

                    }
                    break;
                case "Delete":
                    // delete message from db and send info to all receivers
                    break;
                case "Update":

                    break;

            }
        }
    }
}
