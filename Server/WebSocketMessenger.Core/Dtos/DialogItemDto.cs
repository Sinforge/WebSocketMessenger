namespace WebSocketMessenger.Core.Dtos
{
    public class DialogItemDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public string LastMessage { get; set; }
    }
}
