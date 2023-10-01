using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.DTOs;

namespace WebSockerMessenger.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public User() { }
        public User(UserDTO userDTO)
        {
            Id = Guid.NewGuid();
            Name = userDTO.Name;
            Email = userDTO.Email;
            Surname = userDTO.Surname;
            UserName = userDTO.UserName;
            Password = userDTO.Password;

        }
        public override string ToString()
        {
            return $"User : \nname:{Name}\nsurname:{Surname}\nusername:{UserName}\nemail:{Email}\npassword;{Password}";
        }
    }
}
