using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions
{
    public interface IWebSocketConnectionManager
    {
        public string AddSocket(WebSocket webSocket);
        public ConcurrentDictionary<string, List<WebSocket>> GetAllSockets();
    }
}
