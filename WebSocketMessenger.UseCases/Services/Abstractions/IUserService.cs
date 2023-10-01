using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.DTOs;
using WebSockerMessenger.Core.Models;

namespace WebSocketMessenger.UseCases.Services.Abstractions
{
    public interface IUserService
    {

        public Task<bool> CreateUserAsync(UserDTO userDTO);

        public Task<bool> DeleteUserByIdAsync(Guid id);

        public Task<bool> UpdateUserAsync(User user);

        public Task<User?> FindUserByIdAsync(Guid id);

        public Task<User?> FindUserByUsernameAsync(string username);

        public Task<User?> CheckUserCredentials(LoginDTO loginDTO);
    }
}
