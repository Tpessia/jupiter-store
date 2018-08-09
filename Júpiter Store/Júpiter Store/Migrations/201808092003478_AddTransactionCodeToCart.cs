namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransactionCodeToCart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "TransactionCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "TransactionCode");
        }
    }
}
