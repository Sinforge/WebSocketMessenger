using Microsoft.Extensions.DependencyInjection;
using WebSocketMessenger.UseCases.Services;
using WebSocketMessenger.UseCases.Services.Abstractions;

namespace WebSocketMessenger.UseCases.Extentions
{
    public static class ServiceExtentions
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }

    }
}
