using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.DTOs.WebSocket.Base;

namespace WebSockerMessenger.Core.DTOs.WebSocket.Content
{
    public class UpdateMessageContent : MessageContentBase
    {
        public int MessageId { get; set; }
        public string NewContent { get; set; }

        //public override void Handle()
        //{
        //    base.Handle();
        //}
    }
}
