using WebSocketMessenger.Core.Interfaces.Repositories;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects
{
    public class RepositoryCollection
    {
        public IMessageRepository MessageRepository { get; init; }
        public IGroupRepository GroupRepository { get; init; }
        
        public IUserRepository UserRepository { get; init; }
        public RepositoryCollection(IGroupRepository groupRepository, IMessageRepository messageRepository, IUserRepository userRepository)
        {
            GroupRepository = groupRepository;
            MessageRepository = messageRepository;
            UserRepository = userRepository;
        }
    }
}
