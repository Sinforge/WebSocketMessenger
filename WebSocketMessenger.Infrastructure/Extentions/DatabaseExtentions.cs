using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebSocketMessenger.Infrastructure.Data;
using WebSocketMessenger.Infrastructure.Data.Repositories;
using WebSocketMessenger.Infrastructure.Data.Repositories.Abstractions;
using WebSocketMessenger.Infrastructure.WS.Objects;

namespace WebSocketMessenger.Infrastructure.Extentions
{
    public static class DatabaseExtentions
    {
        public static void AddDapperDatabase(this IServiceCollection services, IConfiguration configuration)
        {
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
                        .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

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
