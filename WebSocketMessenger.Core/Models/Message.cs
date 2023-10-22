namespace WebSocketMessenger.Core.Models
{
    public class Message
    {
        public int Id { get; set; }

        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }

        public string Content { get; set; } = null!;

        // Group or user
        public int MessageType { get; set; }
        //File or text
        public int MessageContentType { get; set; }
        public DateTime SendTime { get; set; }


    }
}
