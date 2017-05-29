namespace Prak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestDel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.jWorkList", "GroupWorkListId");
            DropColumn("dbo.jWorkList", "Relevance");
        }

        public override void Down()
        {
            AddColumn("dbo.jWorkList", "GroupWorkListId", c => c.Int());
            AddColumn("dbo.jWorkList", "Relevance", c => c.Boolean());
        }
    }
}
