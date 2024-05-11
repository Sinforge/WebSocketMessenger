namespace WebSocketMessenger.API.DTOs;

public class KickUserFromGroupRequest
{
    public Guid GroupId { get; set; }
    
    public Guid UserId { get; set; }
}