namespace WebSocketMessenger.Core.DTOs;

public class AddUsersToGroupRequest
{
    public IEnumerable<Guid> Ids { get; set; }
    
    public Guid GroupId { get; set; }
}