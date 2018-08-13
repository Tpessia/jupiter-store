namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCheckoutUriToCart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "CheckoutUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "CheckoutUrl");
        }
    }
}
