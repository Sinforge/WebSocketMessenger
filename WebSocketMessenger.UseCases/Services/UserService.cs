using Microsoft.AspNetCore.Identity;
using WebSocketMessenger.Core.Interfaces.Services;
using WebSocketMessenger.Core.Models;
using WebSocketMessenger.Core.Interfaces.Repositories;

namespace WebSocketMessenger.UseCases.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public async Task<bool> CreateUserAsync(User user)
        {
            if(await _userRepository.FindUserByUsernameAsync(user.UserName) != null ||
                await _userRepository.FindUserByEmailAsync(user.Email) != null)
            {
                return false;
            }
            else
            {
                user.Password = new PasswordHasher<object?>().HashPassword(null, user.Password);
                return await _userRepository.CreateUserAsync(user);
            }
        }

        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            if(await _userRepository.FindUserByIdAsync(id) == null)
            {
                return false;
            }
            else
            {
                return await _userRepository.DeleteUserAsync(id);
            }
        }

        public async Task<User?> FindUserByIdAsync(Guid id)
        {
            return await _userRepository.FindUserByIdAsync(id);
        }

        public async Task<User?> FindUserByUsernameAsync(string username)
        {
            return await _userRepository.FindUserByUsernameAsync(username);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<User?> CheckUserCredentials(string login, string password)
        {
            User? user = await _userRepository.CheckUserCredentials(login);
            var verifyResult =new PasswordHasher<object?>().VerifyHashedPassword(null, user.Password, password);
            if(verifyResult == PasswordVerificationResult.Failed)
            {
                return null;
            }
            else
            {
                return user;
            }

        }
    }
}
