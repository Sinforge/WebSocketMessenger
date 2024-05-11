namespace WebSocketMessenger.API.DTOs;

public class UpdateUserGroupRoleRequest
{
    public Guid GroupId { get; set; }
    
    public Guid UserId { get; set; }
    
    public int RoleId { get; set; }
}