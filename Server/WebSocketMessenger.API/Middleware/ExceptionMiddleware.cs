using WebSocketMessenger.Core.Exceptions;

namespace WebSocketMessenger.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next) {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e){
                object result;
                //context.Response.ContentType = "application/json";
                if (e is SharedException myException)
                {
                    context.Response.StatusCode = myException.StatusCode;
                    result = new
                    {
                        StatusCode = myException.StatusCode,
                        Message = myException.Message
                    };


                }
                else
                {

                    //context.Response.StatusCode = 500;
                    result = new
                    {
                        StatusCode = 500,
                        Message = "We got error on server side...."
                    };
                }
                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}
