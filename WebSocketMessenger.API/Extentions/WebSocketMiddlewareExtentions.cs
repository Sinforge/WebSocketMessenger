using WebSockerMessenger.Core.Interfaces.WS;
using WebSocketMessenger.API.Middleware;
using WebSocketMessenger.Infrastructure.WS;

namespace WebSocketMessenger.API.Extentions
{
    public static class WebSocketMiddlewareExtentions
    {
        public static IApplicationBuilder UseWebSocketServer(this IApplicationBuilder app)
        {

            return app.UseMiddleware<WebSocketMiddleware>();

        }
        public static IServiceCollection AddWebSocketConnectionManager<T>(this IServiceCollection services)
            where T : class, IWebSocketConnectionManager
        {
            services.AddSingleton<IWebSocketConnectionManager, T>();
            services.AddTransient<IWebSocketMessageHandler, MessageHandler>();
            return services;
        }
    }
}
