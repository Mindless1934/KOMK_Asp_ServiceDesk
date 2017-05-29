namespace Prak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Fio", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Fio");
        }
    }
}
