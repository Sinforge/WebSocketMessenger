using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using WebSockerMessenger.Core.Interfaces.WS;
using WebSockerMessenger.Infrastructure.TransferObjets.Base;
using WebSocketMessenger.Infrastructure.WS.TransferObjects;

namespace WebSocketMessenger.Infrastructure.WS
{
    public class MessageHandler : IWebSocketMessageHandler
    {
        private readonly IWebSocketConnectionManager _connectionManager;
        private readonly RepositoryCollection _repositoryCollection;
        public MessageHandler(IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            _repositoryCollection = repositoryCollection;
            _connectionManager = connectionManager;
        }

        public async Task HandleMessage(WebSocketReceiveResult result, byte[] message)
        {
            string messageString;
            if (result.MessageType == WebSocketMessageType.Text)
            {
                messageString = Encoding.UTF8.GetString(message, 0, result.Count);
                IMessageBase<MessageContentBase>? castedToMessageType = AnalizeDynamic(messageString);
                await castedToMessageType.Handle(_connectionManager, _repositoryCollection);


            }




        }
        private IMessageBase<MessageContentBase>? AnalizeDynamic(string json)
        {

            // Получение текущей сборки
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Получение всех типов из текущей сборки
            IEnumerable<Type> types = assembly.GetTypes().Where(t => t.BaseType == typeof(MessageContentBase));

            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);

            // Проходимся по каждому типу
            foreach (Type type in types)
            {
                try
                {
                    if (type.Name.Contains((string)obj.HeaderInfo.Method))
                    {
                        var newType = typeof(MessageBase<>).MakeGenericType(type);
                        return (IMessageBase<MessageContentBase>?)JsonConvert.DeserializeObject(json, newType);
                    }

                }
                catch (JsonException ex)
                {
                    continue;
                }



            }
            return null;


        }

    }
}
