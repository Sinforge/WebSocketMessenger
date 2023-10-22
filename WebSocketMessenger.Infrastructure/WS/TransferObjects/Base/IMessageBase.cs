using WebSocketMessenger.Core.Interfaces.WS;
using WebSocketMessenger.Infrastructure.WS.TransferObjects;

namespace WebSocketMessenger.Infrastructure.TransferObjets.Base
{
    public interface IMessageBase<out T>
        where T : MessageContentBase
    {
        //"Type" : "Private",
        //"Method": "Create",
        //"Content": "Text",
        //"Message": {
        //  "From" : "b7f736b8-d741-4fe0-b6a2-56d0f5be0e6c",
        //  "Content": "Hi vlad 1",
        //  "To": "31b3c71a-54bf-4a56-9bc8-9767e1cc8101"
        // }
        public HeaderInfo HeaderInfo { get; set; }
        public T MessageContent { get; }

        public async Task Handle(IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            await MessageContent.HandleAsync(HeaderInfo, connectionManager, repositoryCollection);
        }


    }
    public class MessageBase<T> : IMessageBase<T> where T : MessageContentBase
    {
        public T MessageCont;
        public HeaderInfo HeaderInfo { get; set; }

        public T MessageContent { get => MessageCont; set => MessageCont = value; }
    }
}
