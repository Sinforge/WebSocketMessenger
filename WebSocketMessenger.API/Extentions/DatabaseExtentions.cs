using FluentMigrator.Runner;
using System.Reflection;
using WebSocketMessenger.Core.Interfaces.Repositories;
using WebSocketMessenger.Infrastructure.Data;
using WebSocketMessenger.Infrastructure.Data.Migrations;
using WebSocketMessenger.Infrastructure.Data.Repositories;
using WebSocketMessenger.Infrastructure.WS.TransferObjects;

namespace WebSocketMessenger.API.Extentions
{
    public static class DatabaseExtentions
    {
        public static void AddDapperDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var assemblyWithMigrations = Assembly.GetAssembly(typeof(InitUserTable_202310011845));
            services.AddSingleton<ApplicationContext>();
            services.AddSingleton<DatabaseManager>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<RepositoryCollection>();
            services.AddLogging(c => c.AddFluentMigratorConsole())
                    .AddFluentMigratorCore()
                    .ConfigureRunner(c => c.AddPostgres()
                        .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
                        .ScanIn(assemblyWithMigrations).For.Migrations());

        }
        public static void CreateMigrations(this IApplicationBuilder app, string dbName)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbService = scope.ServiceProvider.GetRequiredService<DatabaseManager>();
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                try
                {
                    dbService.CreateDatabase(dbName);
                    migrationService.ListMigrations();
                    migrationService.MigrateUp();
                }
                catch
                {
                    //log errors or ...
                    throw;
                }
            }

        }
    }
}
