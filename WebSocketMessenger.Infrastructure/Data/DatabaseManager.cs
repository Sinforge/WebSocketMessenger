using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketMessenger.Infrastructure.Data
{
    public class DatabaseManager
    {
        private readonly ApplicationContext _context;
        public DatabaseManager(ApplicationContext context)
        {
            _context = context;
        }
        public void CreateDatabase(string dbName)
        {
            var query = "SELECT datname FROM pg_catalog.pg_database WHERE lower(datname) = lower(@name);\r\n";
            var parameters = new DynamicParameters();
            parameters.Add("name", dbName);
            using (var connection = _context.CreateConnection())
            {
                var records = connection.Query(query, parameters);
                //create a new database if not exist
                if (!records.Any())
                    connection.Execute($"CREATE DATABASE {dbName}");
            }
        }
    }
}
