using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.Models;

namespace WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(User user);
        Task<User?> FindUserByIdAsync(Guid id);
        Task<User?> FindUserByUsernameAsync(string username);
        Task<User?> FindUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);

        Task<User?> CheckUserCredentials(string password, string login);

    }
}
