using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketMessenger.Infrastructure.Data.Migrations
{
    [Migration(202310081932, "Change password type to varchar(85) for hashing with asp.net identity")]
    public class AlterUserPasswordVarcharCount_202310081932 : Migration
    {
        public override void Down()
        {
            Execute.Sql("alter table public.user " +
                "alter column password type varchar(30)");
        }

        public override void Up()
        {
            Execute.Sql("alter table public.user" +
                " alter column password type varchar(85);");
        }
    }
}
