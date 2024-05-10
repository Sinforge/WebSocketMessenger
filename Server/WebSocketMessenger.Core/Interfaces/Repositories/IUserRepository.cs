using System.Collections;
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

        Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<Guid> ids, bool contains = true);
        Task<IEnumerable<User>> GetUsersByIdsAndSearchStringAsync(IEnumerable<Guid> ids, string searchString, bool contains = true);

    }
}
