using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketMessenger.Infrastructure.WS.Handlers.Abstractions
{
    public interface IWebSocketMessageHandler
    {

        Task HandleMessage(WebSocketReceiveResult result, byte[] message);
    }
}
