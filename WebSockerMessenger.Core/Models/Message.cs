using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSockerMessenger.Core.Models
{
    public class Message
    {
        public int Id { get; set; }

        public DateTime SendTime { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
       
        public string Content { get; set; } = null!;


        // Group or user
        public int MessageType { get; set; }

        //File or text
        public int MessageContentType { get; set; }


    }
}
