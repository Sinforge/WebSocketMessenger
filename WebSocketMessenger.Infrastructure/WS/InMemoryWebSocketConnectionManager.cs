using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using WebSockerMessenger.Core.Interfaces.WS;
using WebSocketMessenger.Core.Interfaces.Repositories;

namespace WebSocketMessenger.Infrastructure.WS
{
    public class InMemoryWebSocketConnectionManager : IWebSocketConnectionManager
    {
        private readonly IGroupRepository _groupRepository;
        public InMemoryWebSocketConnectionManager(IGroupRepository groupRepository) {
            _groupRepository = groupRepository;
        }
        private readonly ConcurrentDictionary<string, List<WebSocket>> _sockets = new ConcurrentDictionary<string, List<WebSocket>>();
        public string AddSocket(WebSocket webSocket, string userId)
        {
            //change to user id
            if (_sockets.ContainsKey(userId))
            {
                _sockets[userId].Add(webSocket);
            }
            else
            {
                _sockets.TryAdd(userId, new List<WebSocket>() { webSocket });
            }
            return userId.ToString();
        }

        public void DeleteSocket(WebSocket webSocket)
        {
            foreach (var socketList in _sockets.Values)
            {
                foreach (var socket in socketList)
                {
                    if (socket == webSocket)
                    {
                        socketList.Remove(socket);
                    }
                }
            }
        }

        private ConcurrentDictionary<string, List<WebSocket>> GetAllSockets() => _sockets;


        private IEnumerable<WebSocket> GetGroupSockets(IEnumerable<Guid> userIds)
        {
            LinkedList<WebSocket> result = new();
            foreach (var socketId in _sockets.Keys)
            {
                if (userIds.Contains(Guid.Parse(socketId)))
                {
                    foreach (WebSocket socket in _sockets[socketId])
                    {
                        result.AddLast(socket);
                    }
                }
            }
            return result;



        }

        private async Task NotifyUserAsync(string userId, byte[] message)
        {
            var data = new ArraySegment<byte>(message);
            if (_sockets.TryGetValue(userId, out var sockets))
            {
                foreach(var socket in _sockets[userId]) {
                    await SendMessageAsync(data, WebSocketMessageType.Text, socket);
                }

            }
            
        }
        private async Task SendMessageAsync(ArraySegment<byte> data, WebSocketMessageType type, WebSocket socket)
        {
            await socket.SendAsync(data,
                        WebSocketMessageType.Text,
                        true,
                        cancellationToken: CancellationToken.None
                    );
        }

        public async Task NotifyGroupAsync(string groupId, byte[] message)
        {
            var data = new ArraySegment<byte>(message);
            IEnumerable<WebSocket> sockets = GetGroupSockets(await _groupRepository.GetUserIdsByGroupAsync(Guid.Parse(groupId)));
            foreach (var socket in sockets)
            {
                await SendMessageAsync(data, WebSocketMessageType.Text, socket);
            }
        }

        public async Task NotifySocketsAsync(string targetId, byte[] message, int type)
        {
            if(type == 1)
            {
                await NotifyUserAsync(targetId, message);
            }
            else if( type == 2)
            {

                await NotifyGroupAsync(targetId, message);
            }
        }
    }
}