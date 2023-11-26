using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketMessenger.Core.Interfaces.Repositories;
using WebSocketMessenger.Core.Models;
using WebSocketMessenger.UseCases.Services;

namespace UseCasesTest
{
    public class UserServiceTests
    {
        [Fact]
        public async Task CreateUserAsync_Fail_EmailOrUsernameExists()
        {
            // Arrange
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(m => m.FindUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(() => new User());
            mockUserRepository.Setup(m => m.FindUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => new User());

            UserService userService = new UserService(mockUserRepository.Object);


            // Act
            bool result = await userService.CreateUserAsync(new User
            {
                UserName = It.IsAny<string>(),
                Email = It.IsAny<string>()
            });


            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task CreateUserAsync_Success()
        {
            // Arrange
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(m => m.FindUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(() => null);
            mockUserRepository.Setup(m => m.FindUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);
            mockUserRepository.Setup(m => m.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(() => true);

            UserService userService = new UserService(mockUserRepository.Object);


            // Act
            bool result = await userService.CreateUserAsync(new User
            {

                Password = "password",
                UserName = It.IsAny<string>(),
                Email = It.IsAny<string>()
            });


            // Assert
            Assert.True(result);
        }
        [Fact]
        public async Task DeleteUserByIdAsync_Fail_UserNotExist()
        {
            // Arrange
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(m => m.FindUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

            UserService userService = new UserService(mockUserRepository.Object);


            // Act
            bool result = await userService.DeleteUserByIdAsync(It.IsAny<Guid>());

            // Assert
            Assert.False(result);

        }

        [Fact]
        public async Task DeleteUserByIdAsync_Success()
        {
            // Arrange
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(m => m.FindUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => new User());
            mockUserRepository.Setup(m => m.DeleteUserAsync(It.IsAny<Guid>())).ReturnsAsync(() => true);
            UserService userService = new UserService(mockUserRepository.Object);


            // Act
            bool result = await userService.DeleteUserByIdAsync(It.IsAny<Guid>());

            // Assert
            Assert.True(result);

        }

        [Fact]
        public async Task CheckUserCredentials_Success()
        {
            // Arrange
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(m => m.CheckUserCredentials(It.IsAny<string>())).ReturnsAsync(() => new User
            {
                Password = new PasswordHasher<object>().HashPassword(null, "password")
            });


            //mockUserRepository.Setup(m => m.DeleteUserAsync(It.IsAny<Guid>())).ReturnsAsync(() => true);
            UserService userService = new UserService(mockUserRepository.Object);


            // Act
            User? result = await userService.CheckUserCredentials(It.IsAny<string>(), "password");

            // Assert
            Assert.NotNull(result);

        }


        [Fact]
        public async Task CheckUserCredentials_Fail_NotCorrectPassword()
        {
            // Arrange
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(m => m.CheckUserCredentials(It.IsAny<string>())).ReturnsAsync(() => new User
            {
                Password = new PasswordHasher<object>().HashPassword(null, "password1")
            });


            //mockUserRepository.Setup(m => m.DeleteUserAsync(It.IsAny<Guid>())).ReturnsAsync(() => true);
            UserService userService = new UserService(mockUserRepository.Object);


            // Act
            User? result = await userService.CheckUserCredentials(It.IsAny<string>(), "password");

            // Assert
            Assert.Null(result);

        }


    }
}
