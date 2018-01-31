namespace PrayashStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PluralizeDBTableNames : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Category", newName: "Categories");
            RenameTable(name: "dbo.ProductImage", newName: "ProductImages");
            RenameTable(name: "dbo.Product", newName: "Products");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Products", newName: "Product");
            RenameTable(name: "dbo.ProductImages", newName: "ProductImage");
            RenameTable(name: "dbo.Categories", newName: "Category");
        }
    }
}
