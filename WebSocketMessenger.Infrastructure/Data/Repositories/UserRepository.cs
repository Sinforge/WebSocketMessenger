using Dapper;
using Microsoft.Extensions.Logging;
using WebSockerMessenger.Core.Models;
using WebSocketMessenger.Core.Interfaces.Repositories;

namespace WebSocketMessenger.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<IUserRepository> _logger;
        public UserRepository(ApplicationContext context, ILogger<IUserRepository> logger) {
            _logger = logger;
            _context = context;
        }
        public async Task<bool> CreateUserAsync(User user)
        {
          
            string insertQuery = "insert into public.user (id, name, surname, username, email, password) values" +
                "(@Id, @Name, @Surname, @UserName, @Email, @Password);";
            try
            {
                using(var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(insertQuery, user);
                }

            }
            catch (Exception)
            {
                _logger.LogWarning($"Cant insert new user: {user}");
                return false;
            }
            return true;
            
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            string deleteQuery = "delete from public.user where id = @id";
            try
            {
                using(var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(deleteQuery, new { id = id });
                }
            }
            catch (Exception)
            {
                _logger.LogWarning($"Cant delete user with id : {id}");
                return false;
            }
            return true;
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            string selectQuery = "select * from public.user where email = @email";
            User? user = null;
            try
            {
                using(var connection = _context.CreateConnection())
                {
                    user = await connection.QuerySingleOrDefaultAsync<User>(selectQuery, new { email = email });
                }
            }
            catch (Exception)
            {
                _logger.LogWarning($"Cant find user with email : {email}");
            }
            return user;
        }

        public async Task<User?> FindUserByIdAsync(Guid id)
        {
            string selectQuery = "select * from public.user where id = @id";
            User? user = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    user = await connection.QuerySingleOrDefaultAsync<User>(selectQuery, new { id = id });
                }
            }
            catch (Exception)
            {
                _logger.LogWarning($"Cant find user with id : {id}");
            }
            return user;
        }

        public async Task<User?> FindUserByUsernameAsync(string username)
        {
            string selectQuery = "select * from public.user where username = @username";
            User? user = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    user = await connection.QuerySingleOrDefaultAsync<User>(selectQuery, new { username = username });
                }
            }
            catch (Exception)
            {
                _logger.LogWarning($"Cant find user with username : {username}");
            }
            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            string updateQuery = "update public.user set name = @Name, " +
                "username = @Username," +
                "email = @Email," +
                "password=  @Password where id = @Id";
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    await connection.QuerySingleOrDefaultAsync<User>(updateQuery, user);
                }
            }
            catch (Exception)
            {
                _logger.LogError($"Cant update user with new data: {user}");
                return false;
            }
            return true;

        }
        public async Task<User?> CheckUserCredentials(string username)
        {
            string selectQuery = "select * from public.user where username = @username";
            User? user = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    user = await connection.QuerySingleOrDefaultAsync<User>(selectQuery, new { username = username });
                }
            }
            catch
            {
                _logger.LogWarning($"Login error");
            }
            return user; 
        }

    }
}
