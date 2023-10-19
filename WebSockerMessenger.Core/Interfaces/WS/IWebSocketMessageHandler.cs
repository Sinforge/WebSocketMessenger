using System.Net.WebSockets;
namespace WebSockerMessenger.Core.Interfaces.WS
{
    public interface IWebSocketMessageHandler
    {
        Task HandleMessage(WebSocketReceiveResult result, byte[] message);
    }
}
