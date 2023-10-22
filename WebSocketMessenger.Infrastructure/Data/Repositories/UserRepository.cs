using Dapper;
using Microsoft.Extensions.Logging;
using WebSocketMessenger.Core.Interfaces.Repositories;
using WebSocketMessenger.Core.Models;

namespace WebSocketMessenger.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<IUserRepository> _logger;
        public UserRepository(ApplicationContext context, ILogger<IUserRepository> logger)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<bool> CreateUserAsync(User user)
        {

            string insertQuery = "insert into public.user (id, name, surname, username, email, password) values" +
                "(@Id, @Name, @Surname, @UserName, @Email, @Password);";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, user);
            }


            return true;

        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            string deleteQuery = "delete from public.user where id = @id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(deleteQuery, new { id = id });
            }

            return true;
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            string selectQuery = "select * from public.user where email = @email";
            User? user = null;

            using (var connection = _context.CreateConnection())
            {
                user = await connection.QuerySingleOrDefaultAsync<User>(selectQuery, new { email = email });
            }

            return user;
        }

        public async Task<User?> FindUserByIdAsync(Guid id)
        {
            string selectQuery = "select * from public.user where id = @id";
            User? user = null;

            using (var connection = _context.CreateConnection())
            {
                user = await connection.QuerySingleOrDefaultAsync<User>(selectQuery, new { id = id });
            }

            return user;
        }

        public async Task<User?> FindUserByUsernameAsync(string username)
        {
            string selectQuery = "select * from public.user where username = @username";
            User? user = null;

            using (var connection = _context.CreateConnection())
            {
                user = await connection.QuerySingleOrDefaultAsync<User>(selectQuery, new { username = username });
            }

            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            string updateQuery = "update public.user set name = @Name, " +
                "username = @Username," +
                "email = @Email," +
                "password=  @Password where id = @Id";

            using (var connection = _context.CreateConnection())
            {
                await connection.QuerySingleOrDefaultAsync<User>(updateQuery, user);
            }

            return true;

        }
        public async Task<User?> CheckUserCredentials(string username)
        {
            string selectQuery = "select * from public.user where username = @username";
            User? user = null;

            using (var connection = _context.CreateConnection())
            {
                user = await connection.QuerySingleOrDefaultAsync<User>(selectQuery, new { username = username });
            }

            return user;
        }

    }
}
