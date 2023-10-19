using WebSockerMessenger.Core.Interfaces.WS;
using WebSocketMessenger.Infrastructure.WS.TransferObjects;

namespace WebSockerMessenger.Infrastructure.TransferObjets.Base
{
    public class MessageContentBase
    {
        public virtual Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection) {
            return Task.CompletedTask;
        }
    }
}
