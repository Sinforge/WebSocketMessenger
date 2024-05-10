namespace WebSocketMessenger.Core.DTOs;

public class UserToInviteDto
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string FirstName { get; set; }
    
    public string SecondName { get; set; }
}