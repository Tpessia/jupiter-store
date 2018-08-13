namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateProducts : DbMigration
    {
        public override void Up()
        {
            Sql(@"
SET IDENTITY_INSERT [dbo].[Products] ON
INSERT INTO [dbo].[Products] ([Id], [Name], [Description], [ImagePath], [Price], [NumberInStock], [DateAdded]) VALUES (1, N'Teste 3', N'Descrição teste 3', N'', 25, 20, N'2018-08-10 23:06:48')
INSERT INTO [dbo].[Products] ([Id], [Name], [Description], [ImagePath], [Price], [NumberInStock], [DateAdded]) VALUES (2, N'Teste 2', N'Descrição teste 2', N'', 14.9, 18, N'2018-08-10 23:07:21')
SET IDENTITY_INSERT [dbo].[Products] OFF
");
        }
        
        public override void Down()
        {
            
        }
    }
}
