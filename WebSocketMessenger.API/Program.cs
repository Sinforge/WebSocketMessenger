using WebSocketMessenger.API.Extentions;
using WebSocketMessenger.Infrastructure.WS;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDapperDatabase(builder.Configuration);
builder.Services.AddWebSocketConnectionManager<InMemoryWebSocketConnectionManager>();
builder.Services.AddUseCases();
builder.Services.AddAuth(builder.Configuration);
var app = builder.Build();
app.UseWebSockets();
app.UseWebSocketServer();
app.CreateMigrations("messenger");
app.MapControllers();
app.Run();
