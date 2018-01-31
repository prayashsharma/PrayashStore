namespace PrayashStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameOrderModelClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Orders", "Date");
            DropColumn("dbo.Orders", "Amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Orders", "Total");
            DropColumn("dbo.Orders", "DateCreated");
        }
    }
}
