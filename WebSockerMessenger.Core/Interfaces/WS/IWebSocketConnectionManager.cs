using System.Collections.Concurrent;
using System.Net.WebSockets;
namespace WebSockerMessenger.Core.Interfaces.WS
{
    public interface IWebSocketConnectionManager
    {
        public string AddSocket(WebSocket webSocket, string userId);
        public void DeleteSocket(WebSocket webSocket);
        public ConcurrentDictionary<string, List<WebSocket>> GetAllSockets();
        public IEnumerable<WebSocket> GetGroupSockets(IEnumerable<Guid> userIds);
    }
}
