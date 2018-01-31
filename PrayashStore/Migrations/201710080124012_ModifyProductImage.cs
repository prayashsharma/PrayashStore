namespace PrayashStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyProductImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductImage", "ImageData", c => c.Binary());
            DropColumn("dbo.ProductImage", "ImagePath");
            DropColumn("dbo.Product", "ThumbnailPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "ThumbnailPath", c => c.String());
            AddColumn("dbo.ProductImage", "ImagePath", c => c.String());
            DropColumn("dbo.ProductImage", "ImageData");
        }
    }
}
