namespace Prak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlzWork : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.jQuery", "GroupQueryId");
            DropColumn("dbo.jQuery", "Relevance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.jQuery", "GroupQueryId", c => c.Int());
            AddColumn("dbo.jQuery", "Relevance", c => c.Boolean());
        }
    }
}
