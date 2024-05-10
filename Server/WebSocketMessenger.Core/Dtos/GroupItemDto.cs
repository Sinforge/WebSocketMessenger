namespace WebSocketMessenger.Core.Dtos;

public class GroupItemDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null !;
    
    public string? LastMessage { get; set; }
    public DateTime? SendTime { get; set; }
}