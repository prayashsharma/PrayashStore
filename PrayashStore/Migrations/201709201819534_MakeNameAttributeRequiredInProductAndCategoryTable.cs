namespace PrayashStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeNameAttributeRequiredInProductAndCategoryTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Category", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Product", "Name", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Category", "Name", c => c.String(maxLength: 255));
        }
    }
}
