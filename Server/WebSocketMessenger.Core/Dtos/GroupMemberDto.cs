namespace WebSocketMessenger.Core.Dtos;

public class GroupMemberDto
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string FirstName { get; set; }
    
    public string SecondName { get; set; }
    
    public string RoleName { get; set; }
    
    public int RoleId { get; set; }
}