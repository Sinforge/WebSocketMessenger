using WebSocketMessenger.API.Extentions;
using WebSocketMessenger.Infrastructure.WS;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials(); ;
        });
});
builder.Services.AddControllers();
builder.Services.AddDapperDatabase(builder.Configuration);
builder.Services.AddWebSocketConnectionManager<InMemoryWebSocketConnectionManager>();
builder.Services.AddUseCases();
builder.Services.AddAuth(builder.Configuration);
var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
//app.UseMyExceptionHandler();
app.UseWebSockets();
app.UseWebSocketServer();
app.CreateMigrations("messenger");
app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}");
app.UseAuthentication();
app.UseAuthorization();
app.Run();
