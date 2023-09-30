using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebSockerMessenger.Core.WebSocketObjects.Abstractions
{
    public abstract class Message
    {
        protected string ReceiverId = null!;
        protected string SenderId = null!;

    }
}
