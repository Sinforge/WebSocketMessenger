namespace WebSocketMessenger.Core.DTOs;

public class GetUsersToInviteRequest
{
    public Guid Id { get; set; }
    public string? SearchString { get; set; }
}