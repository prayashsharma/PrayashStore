namespace PrayashStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            AddColumn("dbo.Product", "ThumbnailPath", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductImage", "ProductId", "dbo.Product");
            DropIndex("dbo.ProductImage", new[] { "ProductId" });
            DropColumn("dbo.Product", "ThumbnailPath");
            DropTable("dbo.ProductImage");
        }
    }
}
