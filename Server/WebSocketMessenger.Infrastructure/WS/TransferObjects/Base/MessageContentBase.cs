using WebSocketMessenger.Core.Interfaces.WS;
using WebSocketMessenger.Infrastructure.WS.TransferObjects;

namespace WebSocketMessenger.Infrastructure.TransferObjets.Base
{
    public class MessageContentBase
    {
        public virtual Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection) {
            return Task.CompletedTask;
        }
    }
}
