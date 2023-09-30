using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Realizations
{
    public class InMemoryWebSocketConnectionManager : IWebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        public string AddSocket(WebSocket webSocket)
        {
            string socketId = Guid.NewGuid().ToString();
            _sockets.TryAdd(socketId, webSocket);
            return socketId;
        }

        public ConcurrentDictionary<string, WebSocket> GetAllSockets() => _sockets;
    }
}