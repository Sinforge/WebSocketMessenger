using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSocketMessenger.Infrastructure.WS.Handlers.Abstractions;

namespace WebSocketMessenger.Infrastructure.WS.Handlers.Realizations
{
    public class CreateMessageHandler : IWebSocketMessageHandler
    {
        public void HandleMessage(WebSocketReceiveResult result, byte[] message)
        {
            
        }
    }
}
