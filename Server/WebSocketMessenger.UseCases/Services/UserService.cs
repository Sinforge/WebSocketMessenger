﻿using Microsoft.AspNetCore.Identity;
using WebSocketMessenger.Core.Interfaces.Services;
using WebSocketMessenger.Core.Models;
using WebSocketMessenger.Core.Interfaces.Repositories;
using WebSocketMessenger.Core.Dtos;

namespace WebSocketMessenger.UseCases.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IMessageRepository _messageRepository;
        public UserService(IUserRepository userRepository, IGroupRepository groupRepository, IMessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _messageRepository = messageRepository;

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

        public async Task<IEnumerable<SearchUserDto>> FindUserByNameAsync(string name)
        {
            return await _userRepository.FindUserByNameAsync(name);
        }

        public async Task<IEnumerable<GroupItemDto>> GetUserGroupsAsync(Guid userId)
        {
            var groupsNames = await _groupRepository.GetUserGroupsAsync(userId);

            var result = new List<GroupItemDto>();
            if (groupsNames.Any())
            {
                var lastMessages = await _messageRepository.GetGroupsLastMessagesAsync(groupsNames.Select(x => x.id));
                if (lastMessages is not null)
                {
                    foreach (var groupName in groupsNames.ToArray())
                    {
                        (Guid id, string message, DateTime sendTime)? lastMessage =
                            lastMessages.FirstOrDefault(x => x.id == groupName.id);
                        result.Add(new GroupItemDto()
                        {
                            Id = groupName.id,
                            Name = groupName.name,
                            LastMessage = lastMessage?.message,
                            SendTime = lastMessage?.sendTime
                        });

                    }
                }
            }

            return result;
        }
    }
}
