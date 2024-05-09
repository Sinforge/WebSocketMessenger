using System.Net.WebSockets;
namespace WebSocketMessenger.Core.Interfaces.WS
{
    public interface IWebSocketMessageHandler
    {
        Task HandleMessage(WebSocketReceiveResult result, WebSocket webSocket,byte[] message);
    }
}
