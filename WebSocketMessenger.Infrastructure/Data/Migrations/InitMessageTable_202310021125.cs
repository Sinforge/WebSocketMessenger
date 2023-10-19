using FluentMigrator;
using FluentMigrator.Postgres;

namespace WebSocketMessenger.Infrastructure.Data.Migrations
{
    [Migration(202310021125, "Initialing of message table")]
    public class InitMessageTable_202310021125 : Migration
    {
        public override void Down()
        {
            Delete.Table("message");
            Delete.Table("message_type");
            Delete.Table("content_type");

        }

        public override void Up()
        {
            Create.Table("message_type")
                .WithColumn("id").AsInt16().PrimaryKey().NotNullable().Identity(PostgresGenerationType.Always)
                .WithColumn("type").AsString(30).Unique().NotNullable();
            Create.Table("content_type")
                .WithColumn("id").AsInt16().PrimaryKey().NotNullable().Identity(PostgresGenerationType.Always)
                .WithColumn("type").AsString(30).Unique().NotNullable();

            Insert.IntoTable("message_type")
                .Row(new
                {
                    id = 1,
                    type = "Private"
                })
                .Row(new
                {
                    id = 2,
                    type = "Group"
                });
            Insert.IntoTable("content_type")
                .Row(new
                {
                    id = 1,
                    type = "Text"
                })
                .Row(new
                {
                    id = 2,
                    type = "File"
                });
            Create.Table("message")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity(PostgresGenerationType.Always)
                .WithColumn("sender_id").AsGuid().NotNullable().ForeignKey("user", "id")
                .WithColumn("receiver_id").AsGuid().NotNullable().ForeignKey("user", "id")
                .WithColumn("content").AsString(150).NotNullable()
                .WithColumn("message_type").AsInt16().NotNullable().ForeignKey("message_type", "id")
                .WithColumn("message_content_type").AsInt16().NotNullable().ForeignKey("content_type", "id");
        }
    }
}
