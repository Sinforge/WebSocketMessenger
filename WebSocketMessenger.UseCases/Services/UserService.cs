using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockerMessenger.Core.DTOs;
using WebSockerMessenger.Core.Models;
using WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions;
using WebSocketMessenger.UseCases.Services.Abstractions;

namespace WebSocketMessenger.UseCases.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public async Task<bool> CreateUserAsync(UserDTO userDTO)
        {
            if(await _userRepository.FindUserByUsernameAsync(userDTO.UserName) != null ||
                await _userRepository.FindUserByEmailAsync(userDTO.Email) != null)
            {
                return false;
            }
            else
            {
                return await _userRepository.CreateUserAsync(new User(userDTO));
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

        public async Task<User?> CheckUserCredentials(LoginDTO loginDTO)
        {
            User? user = await _userRepository.CheckUserCredentials(loginDTO.Login);
            var verifyResult =new PasswordHasher<object?>().VerifyHashedPassword(null, user.Password, loginDTO.Password);
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
