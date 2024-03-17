using FluentMigrator;

namespace WebSocketMessenger.Infrastructure.Data.Migrations
{
    [Migration(202310011845, "Initial of user table")]
    public class InitUserTable_202310011845 : Migration
    {
        public override void Down()
        {
            Delete.Table("user");
        }

        public override void Up()
        {
            Create.Table("user")
             .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
             .WithColumn("name").AsString(30).NotNullable()
             .WithColumn("surname").AsString(30).NotNullable()
             .WithColumn("password").AsString(30).NotNullable()
             .WithColumn("username").AsString(30).NotNullable().Unique()
             .WithColumn("email").AsString(40).NotNullable().Unique();
                
        }
    }
}
