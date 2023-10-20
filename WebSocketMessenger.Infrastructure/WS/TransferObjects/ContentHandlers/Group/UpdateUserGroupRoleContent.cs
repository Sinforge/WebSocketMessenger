using Newtonsoft.Json;
using System.Text;
using WebSockerMessenger.Core.Interfaces.WS;
using WebSockerMessenger.Infrastructure.TransferObjets.Base;

namespace WebSocketMessenger.Infrastructure.WS.TransferObjects.ContentHandlers.Group
{
    public class UpdateUserGroupRoleContent : MessageContentBase
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public override async Task HandleAsync(HeaderInfo header, IWebSocketConnectionManager connectionManager, RepositoryCollection repositoryCollection)
        {
            await repositoryCollection.GroupRepository.UpdateUserGroupRoleAsync(GroupId, UserId, RoleId);
            await connectionManager.NotifySocketsAsync(GroupId.ToString(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this)), 2);

 
        }
    }
}
