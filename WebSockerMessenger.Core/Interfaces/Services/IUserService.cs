﻿using WebSockerMessenger.Core.Models;
namespace WebSockerMessenger.Core.Interfaces.Services
{
    public interface IUserService
    {

        public Task<bool> CreateUserAsync(User user);

        public Task<bool> DeleteUserByIdAsync(Guid id);

        public Task<bool> UpdateUserAsync(User user);

        public Task<User?> FindUserByIdAsync(Guid id);

        public Task<User?> FindUserByUsernameAsync(string username);

        public Task<User?> CheckUserCredentials(string login, string password);
    }
}
