namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseDateToCart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "PurchaseDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "PurchaseDate");
        }
    }
}
