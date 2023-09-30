using WebSocketMessenger.Infrastructure.Extentions;
using WebSocketMessenger.Infrastructure.WS.WebSocketConnectionManager.Realizations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddWebSocketConnectionManager<InMemoryWebSocketConnectionManager>();
var app = builder.Build();
app.UseWebSockets();
app.UseWebSocketServer();

app.Run();
