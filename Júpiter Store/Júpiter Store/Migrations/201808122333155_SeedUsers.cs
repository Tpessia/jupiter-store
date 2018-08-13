namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"

INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName]) VALUES (N'2f5a7c09-2d5f-4898-847a-092683f4d60d', N'admin@jupiter.com', 0, N'AOILrzsHQeAHj6Tj1tkDZKRq8o7BlVhvy3QweBJ4Vaz5C3x+JfAqj0TcoNo4CpunNQ==', N'6d95b42b-f056-4ae7-a98c-52751b024604', N'(11)949911003', 0, 0, NULL, 1, 0, N'admin@jupiter.com', N'Admin', N'User')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName]) VALUES (N'c71052f3-9614-45a9-88dd-5224e9ec3ab1', N'test@jupiter.com', 0, N'AHtyqdzyOys6x2TRgLpqqlC9un9Jdn3ZhchjmkCReWMH/ENOOjolSKf10HkD4xTJpw==', N'dbb589ed-7a17-4242-8a4c-798ae0e52188', N'(11)949911003', 0, 0, NULL, 1, 0, N'test@jupiter.com', N'Test', N'User')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName]) VALUES (N'e8a02ca0-d4df-4439-8a23-52dab6efecef', N'manager@jupiter.com', 0, N'AEj95yImfUjVqjleUFoocTr0OM2axDa4Zbj0+YD1kQU1wCHrfrGZ4Hsv9KIx6HHWVg==', N'e1f9d902-d88c-48f6-9e87-a825b351a927', N'(11)949911003', 0, 0, NULL, 1, 0, N'manager@jupiter.com', N'Manager', N'User')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'8a214663-749b-4114-94ed-0a27ce454d09', N'Admin')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'33b68ead-018b-4827-8ed3-a2b85e8373b3', N'Manager')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'2f5a7c09-2d5f-4898-847a-092683f4d60d', N'33b68ead-018b-4827-8ed3-a2b85e8373b3')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e8a02ca0-d4df-4439-8a23-52dab6efecef', N'33b68ead-018b-4827-8ed3-a2b85e8373b3')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'2f5a7c09-2d5f-4898-847a-092683f4d60d', N'8a214663-749b-4114-94ed-0a27ce454d09')

SET IDENTITY_INSERT [dbo].[Carts] ON
INSERT INTO [dbo].[Carts] ([Id], [IsActive], [ApplicationUser_Id], [PurchaseDate]) VALUES (1, 1, N'2f5a7c09-2d5f-4898-847a-092683f4d60d', NULL)
INSERT INTO [dbo].[Carts] ([Id], [IsActive], [ApplicationUser_Id], [PurchaseDate]) VALUES (2, 1, N'e8a02ca0-d4df-4439-8a23-52dab6efecef', NULL)
INSERT INTO [dbo].[Carts] ([Id], [IsActive], [ApplicationUser_Id], [PurchaseDate]) VALUES (3, 1, N'c71052f3-9614-45a9-88dd-5224e9ec3ab1', NULL)
SET IDENTITY_INSERT [dbo].[Carts] OFF

");
        }
        
        public override void Down()
        {
        }
    }
}
