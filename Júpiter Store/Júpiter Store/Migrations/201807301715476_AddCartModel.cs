namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCartModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCarts",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        CartId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.CartId })
                .ForeignKey("dbo.Carts", t => t.CartId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.CartId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Cart_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "Cart_Id");
            AddForeignKey("dbo.AspNetUsers", "Cart_Id", "dbo.Carts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.ProductCarts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductCarts", "CartId", "dbo.Carts");
            DropIndex("dbo.AspNetUsers", new[] { "Cart_Id" });
            DropIndex("dbo.ProductCarts", new[] { "CartId" });
            DropIndex("dbo.ProductCarts", new[] { "ProductId" });
            DropColumn("dbo.AspNetUsers", "Cart_Id");
            DropTable("dbo.Carts");
            DropTable("dbo.ProductCarts");
        }
    }
}
