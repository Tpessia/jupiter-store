namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProductImagesLocation : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE Products SET ImagePath = CONCAT('~\Public\Images\Products\', REVERSE(SUBSTRING(REVERSE(ImagePath), 0, CHARINDEX('\', REVERSE(ImagePath)))))");
        }
        
        public override void Down()
        {
        }
    }
}
