using System.Collections.Concurrent;
using System.Net.WebSockets;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Realizations
{
    public class InMemoryWebSocketConnectionManager : IWebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, List<WebSocket>> _sockets = new ConcurrentDictionary<string, List<WebSocket>>();
        public string AddSocket(WebSocket webSocket, string userId)
        {
            //change to user id
            if(_sockets.ContainsKey(userId))
            {
                _sockets[userId].Add(webSocket);
            }
            else
            {
                _sockets.TryAdd(userId, new List<WebSocket>() { webSocket});
            }
            return userId.ToString();
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