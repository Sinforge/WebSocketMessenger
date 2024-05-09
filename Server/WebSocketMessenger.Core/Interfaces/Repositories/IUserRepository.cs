using WebSocketMessenger.Core.Dtos;
using WebSocketMessenger.Core.Models;
namespace WebSocketMessenger.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(User user);
        Task<User?> FindUserByIdAsync(Guid id);
        Task<User?> FindUserByUsernameAsync(string username);
        Task<User?> FindUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<User?> CheckUserCredentials(string login);
        Task<IEnumerable<SearchUserDto>> FindUserByNameAsync(string name);

        Task<string> GetUsernameByIdAsync(Guid id);
    }
}
