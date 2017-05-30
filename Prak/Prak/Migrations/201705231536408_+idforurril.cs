namespace Prak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idforurril : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUserRoles", "UserRoleId", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUserRoles", "UserRoleId");
        }
    }
}
