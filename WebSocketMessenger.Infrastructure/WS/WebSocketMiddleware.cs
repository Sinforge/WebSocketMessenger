using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using WebSocketMessenger.Infrastructure.WS.Handlers.Abstractions;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;

namespace WebSocketMessenger.WS
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
            var buffer = new byte[1024 * 4];
            while(ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(buffer :new ArraySegment<byte>(buffer),
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
                if (result.Succeeded) {
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    _connectionManager.AddSocket(webSocket, result?.Principal?.Claims?.FirstOrDefault(c => c.Type == "Id")?.Value);
                    await ReceiveMessage(webSocket);
                }
                else
                {
                    await _next(context);
                }
                
                
                //await ReceiveMessage(webSocket, async (result, buffer) =>
                //{
                //    if(result.MessageType == WebSocketMessageType.Text)
                //    {
                //        Console.WriteLine();
                //        return;
                //    }
                //    else if(result.MessageType == WebSocketMessageType.Close)
                //    {
                //        var socket  = _connectionManager.GetAllSockets().FirstOrDefault(kv => kv.Key == socketId);
                //        _connectionManager.GetAllSockets().TryRemove(socketId, out WebSocket ws);
                //        //ws.SendAsync("");
                //    }
                //    else
                //    {   

                //    }

                //});
                
            }
            else
            {
                await _next(context);
            }
        }

        
        
    }
}
