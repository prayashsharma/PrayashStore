namespace PrayashStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThumbnailToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Thumbnail", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Thumbnail");
        }
    }
}
