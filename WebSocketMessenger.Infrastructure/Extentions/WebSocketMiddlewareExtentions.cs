﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebSocketMessenger.Infrastructure.WS.Handlers.Realizations;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Abstractions;
using WebSocketMessenger.WS;

namespace WebSocketMessenger.Infrastructure.Extentions
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
            services.AddScoped<CreateMessageHandler>();
            services.AddScoped<DeleteMessageHandler>();
            services.AddScoped<UpdateMessageHandler>();
            return services.AddSingleton<IWebSocketConnectionManager, T>();
        }
    }
}
