namespace Prak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class com : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.jWorkList", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.jWorkList", "Comment");
        }
    }
}
