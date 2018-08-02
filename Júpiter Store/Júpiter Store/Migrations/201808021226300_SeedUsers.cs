namespace Júpiter_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"

INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0b40d825-d528-4a8a-be87-15c09ce91bfc', N'manager@jupiter.com', 0, N'AIvckH7pA5gps5bgFyirzHZUhUwxW5XrHhD1qLinl0+M/Hw5WZE/jAC25h0qsOAI5A==', N'cbe2e1e3-17dd-4412-b6ed-508e45feae01', NULL, 0, 0, NULL, 1, 0, N'manager@jupiter.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'b520e275-4794-432f-9a72-4569f750e9d5', N'teste@jupiter.com', 0, N'AL0hxo5Nf/PNqG3GTLX8b99NZsyBYMcrAt6pKtjAfl/q/2BhhmmaYUR7t4FVDTxnYQ==', N'aa603be1-2187-43d4-bd45-1d6d951ba5f4', NULL, 0, 0, NULL, 1, 0, N'teste@jupiter.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'e60cff22-1fda-4b94-9c09-c1f8c1ced60f', N'admin@jupiter.com', 0, N'AHYUUzClTFazFEIfQaIKBrzjEEcSXQKEdCceBu2Ci1Mk9DDO+tdNIN+SYHkPcI79ZA==', N'683eb3bb-030f-4907-bcb6-8251153bcc23', NULL, 0, 0, NULL, 1, 0, N'admin@jupiter.com')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'8a214663-749b-4114-94ed-0a27ce454d09', N'Admin')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'33b68ead-018b-4827-8ed3-a2b85e8373b3', N'Manager')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0b40d825-d528-4a8a-be87-15c09ce91bfc', N'33b68ead-018b-4827-8ed3-a2b85e8373b3')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e60cff22-1fda-4b94-9c09-c1f8c1ced60f', N'8a214663-749b-4114-94ed-0a27ce454d09')

SET IDENTITY_INSERT [dbo].[Carts] ON
    INSERT INTO [dbo].[Carts] ([Id], [IsActive], [ApplicationUser_Id], [PurchaseDate]) VALUES (1, 1, N'0b40d825-d528-4a8a-be87-15c09ce91bfc', NULL)
    INSERT INTO [dbo].[Carts] ([Id], [IsActive], [ApplicationUser_Id], [PurchaseDate]) VALUES (2, 1, N'b520e275-4794-432f-9a72-4569f750e9d5', NULL)
    INSERT INTO [dbo].[Carts] ([Id], [IsActive], [ApplicationUser_Id], [PurchaseDate]) VALUES (3, 1, N'e60cff22-1fda-4b94-9c09-c1f8c1ced60f', NULL)
SET IDENTITY_INSERT [dbo].[Carts] OFF

");
        }
        
        public override void Down()
        {
        }
    }
}
