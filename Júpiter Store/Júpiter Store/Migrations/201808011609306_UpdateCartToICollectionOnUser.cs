namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCartToICollectionOnUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Cart_Id", "dbo.Carts");
            DropIndex("dbo.AspNetUsers", new[] { "Cart_Id" });
            AddColumn("dbo.Carts", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Carts", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Carts", "ApplicationUser_Id");
            AddForeignKey("dbo.Carts", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "Cart_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Cart_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Carts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Carts", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Carts", "ApplicationUser_Id");
            DropColumn("dbo.Carts", "IsActive");
            CreateIndex("dbo.AspNetUsers", "Cart_Id");
            AddForeignKey("dbo.AspNetUsers", "Cart_Id", "dbo.Carts", "Id", cascadeDelete: true);
        }
    }
}
