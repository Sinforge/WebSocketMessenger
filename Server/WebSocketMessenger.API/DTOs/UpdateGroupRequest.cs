namespace WebSocketMessenger.API.DTOs;

public class UpdateGroupRequest
{
    public Guid GroupId { get; set; }
    public string Name { get; set; }
}