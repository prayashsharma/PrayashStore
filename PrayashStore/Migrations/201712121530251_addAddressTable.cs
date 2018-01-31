namespace PrayashStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAddressTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Attn = c.String(maxLength: 70),
                        Line1 = c.String(nullable: false, maxLength: 70),
                        Line2 = c.String(maxLength: 70),
                        City = c.String(nullable: false, maxLength: 40),
                        State = c.String(nullable: false, maxLength: 2),
                        ZipCode = c.String(nullable: false, maxLength: 10),
                        Country = c.String(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Addresses", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Addresses", new[] { "ApplicationUserId" });
            DropTable("dbo.Addresses");
        }
    }
}
