using System.Net.WebSockets;
namespace WebSocketMessenger.Core.Interfaces.WS
{
    public interface IWebSocketConnectionManager
    {
        public string AddSocket(WebSocket webSocket, string userId);
        public void DeleteSocket(WebSocket webSocket);
        
        // type -> 1 - to one person ; 2 - for group
        public Task NotifySocketsAsync(string targetId, byte[] message, int type);
    }
}
