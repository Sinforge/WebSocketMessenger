using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSockerMessenger.Core.DTOs
{
    public class LoginDTO
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!; 
    }
}
