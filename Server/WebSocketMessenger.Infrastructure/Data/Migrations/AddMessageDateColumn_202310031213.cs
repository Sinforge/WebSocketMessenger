using FluentMigrator;
namespace WebSocketMessenger.Infrastructure.Data.Migrations
{              
    [Migration(202310031213, "Add send_data columng")]
    public class AddMessageDateColumn_202310031213 : Migration
    {
        public override void Down()
        {
            Delete.Column("send_date").FromTable("message");
        }

        public override void Up()
        {
            Alter.Table("message")
                .AddColumn("send_date").AsDateTime().NotNullable();
        }
    }
}
