using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using WebSocketMessenger.Core.Interfaces.WS;
using WebSocketMessenger.Infrastructure.FileSystem;
using WebSocketMessenger.Infrastructure.TransferObjets.Base;
using WebSocketMessenger.Infrastructure.WS.TransferObjects;
using WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Conversation.File;

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

        public async Task HandleMessage(WebSocketReceiveResult result, WebSocket socket,byte[] message)
        {
            string messageString;
            if (result.MessageType == WebSocketMessageType.Text)
            {
                messageString = Encoding.UTF8.GetString(message, 0, result.Count);
                IMessageBase<MessageContentBase>? castedToMessageType = AnalizeDynamic(messageString);

                if (castedToMessageType is MessageBase<CreateFileContent> fileMessage)
                {
                    var messageContent = new CreateFileContent()
                    {
                        FileName = await ReceiveFullFileAsync(socket, fileMessage.MessageContent.OriginalName),
                    };
                    fileMessage.MessageContent = messageContent ;
                    castedToMessageType = fileMessage;
                }
                await castedToMessageType.Handle(_connectionManager, _repositoryCollection);
            }




        }
        private IMessageBase<MessageContentBase>? AnalizeDynamic(string json)
        {

            // Получение текущей сборки
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Получение всех типов из текущей сборки
            IEnumerable<Type> types = assembly.GetTypes().Where(t => t.BaseType == typeof(MessageContentBase));

            dynamic? obj = JsonConvert.DeserializeObject<dynamic>(json);

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

        private async Task<string> ReceiveFullFileAsync(WebSocket ws, string originalName)
        {
            FileStream fileStream = null;
            string fileName = $"{Guid.NewGuid()}_{originalName}";
            try
            {
                fileStream = FileManager.CreateFileStream(fileName);
                var buffer = new byte[1024 * 120];
                while (true)
                {
                    var bytesReceived = await ws.ReceiveAsync(buffer, CancellationToken.None);
                    if (bytesReceived.Count == 0)
                    {
                        break;
                    }

                    await fileStream.WriteAsync(buffer, 0, bytesReceived.Count);
                }
                Console.WriteLine(fileStream);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                fileStream?.Close();
                
            }

            return fileName;

        }

    }
}
