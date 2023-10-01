using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSockerMessenger.Core.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;    
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
