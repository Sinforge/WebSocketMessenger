namespace WebSocketMessenger.Core.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Content { get; set; }
        public DateTime SendTime { get; set; }

        public int MessageContentType { get; set; }
       
    }
}
