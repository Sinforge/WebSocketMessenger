using FluentMigrator;

namespace WebSocketMessenger.Infrastructure.Data.Migrations
{
    [Migration(202310052151, "Initialize role table for groups")]
    public class InitGroupRoleTable_202310052151 : Migration
    {
        public override void Down()
        {
            Execute.Sql("alter table public.user_group" +
                "drop column role_id;");
            Execute.Sql("drop table public.group_role;");
        }

        public override void Up()
        {
            Execute.Sql("create public.table group_role(" +
                "id int primary key not null," +
                "name varchar(30) not null" +
                ");");
            Execute.Sql("insert into public.group_role values" +
                "(1, 'Creator')," +
                "(2, 'Moderator')," +
                "(3, 'DefaultUser');"
                );

            Execute.Sql("alter table public.user_group" +
                " add column role_id int;");
            Execute.Sql(
                "alter table public.user_group " +
                "add foreign key(role_id) references group_role(id);");
        }
    }
}
