using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WebSocketMessenger.Core.Interfaces.Repositories;
using WebSocketMessenger.Core.Interfaces.WS;

namespace WebSocketMessenger.Infrastructure.WS
{
    public class InMemoryWebSocketConnectionManager : IWebSocketConnectionManager
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ConcurrentDictionary<string, List<WebSocket>> _sockets = new ConcurrentDictionary<string, List<WebSocket>>();

        public InMemoryWebSocketConnectionManager(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public string AddSocket(WebSocket webSocket, string userId)
        {
            var sockets = _sockets.GetOrAdd(userId, _ => new List<WebSocket>());
            sockets.Add(webSocket);
            return userId.ToString();
        }

        public void DeleteSocket(WebSocket webSocket)
        {
            foreach (var entry in _sockets)
            {
                lock (entry.Value)
                {
                    entry.Value.Remove(webSocket);
                }
            }
        }

        private IEnumerable<WebSocket> GetGroupSockets(IEnumerable<Guid> userIds)
        {
            var result = new List<WebSocket>();
            foreach (var userId in userIds)
            {
                if (_sockets.TryGetValue(userId.ToString(), out var sockets))
                {
                    result.AddRange(sockets);
                }
            }
            return result;
        }

        private async Task NotifyUserAsync(string userId, byte[] message)
        {
            var data = new ArraySegment<byte>(message);
            if (_sockets.TryGetValue(userId, out var sockets))
            {
                foreach (var socket in sockets.ToList())
                {
                    try
                    {
                        await SendMessageAsync(data, WebSocketMessageType.Text, socket);
                    }
                    catch
                    {
                        DeleteSocket(socket);
                    }
                }
            }
        }

        private async Task SendMessageAsync(ArraySegment<byte> data, WebSocketMessageType type, WebSocket socket)
        {
            await socket.SendAsync(data, type, true, CancellationToken.None);
        }

        public async Task NotifyGroupAsync(string groupId, byte[] message)
        {
            var data = new ArraySegment<byte>(message);
            var userIds = await _groupRepository.GetUserIdsByGroupAsync(Guid.Parse(groupId));
            var sockets = GetGroupSockets(userIds);
            foreach (var socket in sockets.ToList())
            {
                await SendMessageAsync(data, WebSocketMessageType.Text, socket);
            }
        }

        public async Task NotifySocketsAsync(string targetId, byte[] message, int type)
        {
            if (type == 1)
            {
                await NotifyUserAsync(targetId, message);
            }
            else if (type == 2)
            {
                await NotifyGroupAsync(targetId, message);
            }
        }
    }
}
