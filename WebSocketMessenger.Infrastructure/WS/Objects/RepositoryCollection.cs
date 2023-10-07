using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions;

namespace WebSocketMessenger.Infrastructure.WS.Objects
{
    public class RepositoryCollection
    {
        public IMessageRepository MessageRepository { get; init; }
        public IGroupRepository GroupRepository { get; init; }
        public RepositoryCollection(IGroupRepository groupRepository, IMessageRepository messageRepository)
        {
            GroupRepository = groupRepository;
            MessageRepository = messageRepository;
        }
    }
}
