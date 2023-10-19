using FluentMigrator;

namespace WebSocketMessenger.Infrastructure.Data.Migrations
{
    [Migration(202310182139, "Change names")]
    public class AlterMessageColumnNames_202310182139 : Migration
    {
        public override void Down()
        {
            Execute.Sql("alter table public.message rename column \"Id' to id;");
                                                                  
            Execute.Sql("alter table public.message rename column \"SenderId\" to sender_id");
            Execute.Sql("alter table public.message rename column \"ReceiverId\" to receiver_id;");
            Execute.Sql("alter table public.message rename column \"Content\" to content;");
            Execute.Sql("alter table public.message rename column \"MessageContentType\" to message_content_type;");
            Execute.Sql("alter table public.message rename column \"MessageType\" to message_type;");
            Execute.Sql("alter table public.message rename column \"SendTime\" to send_date;");
        }

        public override void Up()
        {
            Execute.Sql("alter table public.message rename column id to \"Id\";");
            Execute.Sql("alter table public.message rename column sender_id to \"SenderId\";");
            Execute.Sql("alter table public.message rename column receiver_id to \"ReceiverId\";");
            Execute.Sql("alter table public.message rename column content to \"Content\";");
            Execute.Sql("alter table public.message rename column message_content_type to \"MessageContentType\";");
            Execute.Sql("alter table public.message rename column message_type to \"MessageType\";");
            Execute.Sql("alter table public.message rename column send_date to \"SendTime\";");
            
        }
    }
}
