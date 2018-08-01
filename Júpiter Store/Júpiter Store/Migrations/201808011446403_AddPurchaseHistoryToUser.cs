namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseHistoryToUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatePurchased = c.DateTime(nullable: false),
                        Cart_Id = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carts", t => t.Cart_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.Cart_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.Carts", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseHistories", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PurchaseHistories", "Cart_Id", "dbo.Carts");
            DropIndex("dbo.PurchaseHistories", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.PurchaseHistories", new[] { "Cart_Id" });
            DropColumn("dbo.Carts", "Discriminator");
            DropTable("dbo.PurchaseHistories");
        }
    }
}
