using WebSocketMessenger.Core.Interfaces.Services;
using WebSocketMessenger.UseCases.Services;

namespace WebSocketMessenger.API.Extentions
{
    public static class ServiceExtentions
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IGroupService, GroupService>();
        }
    }
}
