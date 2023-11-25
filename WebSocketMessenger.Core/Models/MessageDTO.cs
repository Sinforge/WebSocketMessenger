using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketMessenger.Core.Models
{
    public class MessageDTO
    {
        public int Type { get; set; }
        public string Message { get; set; }
    }
}
