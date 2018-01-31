namespace PrayashStore.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class LoadDataToCategoryTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO CATEGORY (Name) VALUES ('Shirts')");
            Sql("INSERT INTO CATEGORY (Name) VALUES ('Tees')");
            Sql("INSERT INTO CATEGORY (Name) VALUES ('Pants')");
            Sql("INSERT INTO CATEGORY (Name) VALUES ('Shoes')");
            Sql("INSERT INTO CATEGORY (Name) VALUES ('Accessories')");
        }

        public override void Down()
        {
            Sql("DELETE FROM CATEGORY WHERE Id in (1, 2, 3, 4, 5)");
        }
    }
}
