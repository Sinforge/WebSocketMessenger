using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using System.Net.WebSockets;
using WebSockerMessenger.Core.Interfaces.WS;

namespace WebSocketMessenger.API.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketMessageHandler _handler;
        private readonly IWebSocketConnectionManager _connectionManager;
        public WebSocketMiddleware(RequestDelegate next, IWebSocketMessageHandler handler, IWebSocketConnectionManager connectionManager)
        {
            _handler = handler;
            _connectionManager = connectionManager;
            _next = next;
        }

        private async Task ReceiveMessage(WebSocket ws)
        {
            var buffer = new byte[1024 * 5 * 1024];
            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _connectionManager.DeleteSocket(ws);
                }
                else
                {
                    await _handler.HandleMessage(result, buffer);
                }
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var result = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
                if (result.Succeeded)
                {
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    _connectionManager.AddSocket(webSocket, result?.Principal?.Claims?.FirstOrDefault(c => c.Type == "Id")?.Value);
                    await ReceiveMessage(webSocket);
                }
                else
                {
                    throw new HttpRequestException(message: "You should be authorized to use websocket", null, HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                await _next(context);
            }
        }



    }
}
