using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketMessenger.Infrastructure.Data.Migrations
{
    [Migration(202301101845, "Initial of user table")]
    public class InitUserTable_202301101845 : Migration
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
