namespace DataManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoadTypeFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoadTypes", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastUpdated");
            DropColumn("dbo.LoadTypes", "Active");
        }
    }
}
