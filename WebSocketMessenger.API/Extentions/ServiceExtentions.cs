using WebSocketMessenger.UseCases.Services;
using WebSockerMessenger.Core.Interfaces.Services;

namespace WebSocketMessenger.API.Extentions
{
    public static class ServiceExtentions
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
