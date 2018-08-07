namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateProducts : DbMigration
    {
        public override void Up()
        {
            //Sql("INSERT INTO Products (Name, Description, ImagePath, Price, NumberInStock, DateAdded) VALUES ('Teste 1', 'Descrição teste 1', '~\\Images\\Teste_1_1.jpg', 10, 20, '01/01/2018 00:00:00')");
            //Sql("INSERT INTO Products (Name, Description, ImagePath, Price, NumberInStock, DateAdded) VALUES ('Teste 2', 'Descrição teste 2', '~\\Images\\Teste_2_2.jpg', 14.90, 15, '01/01/2018 00:01:00')");
            //Sql("INSERT INTO Products (Name, Description, ImagePath, Price, NumberInStock, DateAdded) VALUES ('Teste 3', 'Descrição teste 3', '~\\Images\\Teste_3_3.jpg', 25, 10, '01/01/2018 00:02:00')");
        }
        
        public override void Down()
        {
            //Sql("DELETE FROM Products WHERE Name LIKE 'Teste%' AND Description LIKE 'Descrição teste%'");
        }
    }
}
