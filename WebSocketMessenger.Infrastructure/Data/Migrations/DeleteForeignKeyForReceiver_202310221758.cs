using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketMessenger.Infrastructure.Data.Migrations
{
    [Migration(202310221758, "Delete foreign key ")]
    public class DeleteForeignKeyForReceiver_202310221758 : Migration
    {
        public override void Down()
        {
            Execute.Sql("alter table public.message add constraint \"FK_message_receiver_id_user_id\" FOREIGN KEY (\"ReceiverId\") REFERENCES public.user (id);");  
        }

        public override void Up()
        {
            Execute.Sql("alter table public.message drop constraint \"FK_message_receiver_id_user_id\";");
        }
    }
}
