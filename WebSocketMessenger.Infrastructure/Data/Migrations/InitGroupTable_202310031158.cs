using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketMessenger.Infrastructure.Data.Migrations
{
    [Migration(202310031158, "Create group's tables")]
    public class InitGroupTable_202310031158 : Migration
    {
        public override void Down()
        {
            Delete.Table("group");
            Delete.Table("user_group");
        }

        public override void Up()
        {
            Create.Table("group")
                .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("name").AsString(40).NotNullable();
            Create.Table("user_group")
                .WithColumn("group_id").AsGuid().NotNullable().ForeignKey("group", "id")
                .WithColumn("user_id").AsGuid().NotNullable().ForeignKey("user", "id");
            Create.PrimaryKey("pk_user_group").OnTable("user_group").Columns("group_id", "user_id");

            Create.Index("ix_user_group_group_id").OnTable("user_group")
                .OnColumn("group_id");

            Create.Index("ix_user_group_user_id").OnTable("user_group")
                .OnColumn("user_id");

        }
    }
}
