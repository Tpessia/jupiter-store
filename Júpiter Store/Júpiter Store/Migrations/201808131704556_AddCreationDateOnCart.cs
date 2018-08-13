namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreationDateOnCart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "CreationDate");
        }
    }
}
