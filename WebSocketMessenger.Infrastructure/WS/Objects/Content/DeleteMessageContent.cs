using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.DTOs.WebSocket.Base;

namespace WebSockerMessenger.Core.DTOs.WebSocket.Content
{
    public class DeleteMessageContent : MessageContentBase
    {
        public Guid MessageId { get; set; }

        //public override void Handle()
        //{
        //    base.Handle();
        //}
    }
}
