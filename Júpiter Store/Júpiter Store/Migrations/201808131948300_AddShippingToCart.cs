namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShippingToCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartShippings",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Address_Country = c.String(),
                        Address_State = c.String(),
                        Address_City = c.String(),
                        Address_District = c.String(),
                        Address_PostalCode = c.String(),
                        Address_Street = c.String(),
                        Address_Number = c.String(),
                        Address_Complement = c.String(),
                        ShippingType = c.Int(),
                        Cost = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carts", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartShippings", "Id", "dbo.Carts");
            DropIndex("dbo.CartShippings", new[] { "Id" });
            DropTable("dbo.CartShippings");
        }
    }
}
