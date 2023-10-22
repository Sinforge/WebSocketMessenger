namespace WebSocketMessenger.Infrastructure.TransferObjets.Base
{
    public class HeaderInfo
    {
        public Guid From { get; set; }
        public Guid To { get; set; } 
        // 1 - Private, 2 - Group
        public byte Type { get; set; }

        // 1 - Text, 2 - File
        public byte Content { get; set; }
        public DateTime SendTime { get; set; }

    }
}
