namespace Prak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jJur : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.jJournal", "QueryID", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.jJournal", "QueryID");
        }
    }
}
