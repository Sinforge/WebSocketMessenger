using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.Models;

namespace WebSockerMessenger.Core.Utils
{
    public class MessageFactory
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
        public MessageFactory() { }


        public Message CreateMessage(dynamic json)
        {
            string messageType = json.Type;
            string messageContent = json.Content;
            if (messageContent == "Text")
            {
                return new Message()
                {
                    Content = json.Message.Data.Content,
                    MessageType = messageType == "Private" ? 1 : 2,
                    MessageContentType = 1,
                    SenderId = Guid.Parse((string)json.Message.Data.From),
                    ReceiverId = Guid.Parse((string)json.Message.Data.To)
                };
            }
            else if (messageContent == "File")
            {
                string path = $"C:\\Users\\vladv\\source\\repos\\WebSocketMessenger\\Upload\\{Guid.NewGuid().ToString() + (string)json.Message.Data.Type}";
                File.WriteAllBytesAsync(path, Convert.FromBase64String((string)json.Message.Data.Content));

                return new Message()
                {
                    //TODO insert files in folder
                    Content = path,
                    MessageType = messageType == "Private" ? 1 : 2,
                    MessageContentType = 2,
                    SenderId = Guid.Parse((string)json.Message.Data.From),
                    ReceiverId = Guid.Parse((string)json.Message.Data.To)
                };
            }
            else
            {
                throw new Exception("Unsupported type");
            }
            return null;
            
        }


    }
}
