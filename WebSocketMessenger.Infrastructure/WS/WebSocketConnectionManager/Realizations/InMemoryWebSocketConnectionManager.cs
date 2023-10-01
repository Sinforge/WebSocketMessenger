using System.Collections.Concurrent;
using System.Net.WebSockets;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Realizations
{
    public class InMemoryWebSocketConnectionManager : IWebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, List<WebSocket>> _sockets = new ConcurrentDictionary<string, List<WebSocket>>();
        public string AddSocket(WebSocket webSocket)
        {
            //change to user id
            string socketId = Guid.NewGuid().ToString();
            if(_sockets.ContainsKey(socketId))
            {
                _sockets[socketId].Add(webSocket);
            }
            else
            {
                _sockets.TryAdd(socketId, new List<WebSocket>() { webSocket});
            }
            return socketId;
        }

        public void DeleteSocket(WebSocket webSocket)
        {
            foreach(var socketList  in _sockets.Values)
            {
                foreach(var socket in socketList)
                {
                    if(socket == webSocket)
                    {
                        socketList.Remove(socket);
                    }
                }
            }
        }

        public ConcurrentDictionary<string, List<WebSocket>> GetAllSockets() => _sockets;
    }
}