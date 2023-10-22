using WebSocketMessenger.API.Middleware;

namespace WebSocketMessenger.API.Extentions
{
    public static class ExceptionMiddlewareExtention
    {
        public static void UseMyExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            
        }
    }
}
