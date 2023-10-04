using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketMessenger.Infrastructure.Data.Migrations
{
    [Migration(202310031213)]
    internal class AddMessageDateColumn_202310031213 : Migration
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
