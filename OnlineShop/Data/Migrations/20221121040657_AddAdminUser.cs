using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [FirstName], [LastName], [ProfilePicture], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'5ee30507-dc59-45b6-900e-2a5ad0f5a026', N'Admin', N'Admin', NULL, N'Admin', N'ADMIN', N'Admin@test.com', N'ADMIN@TEST.COM', 1, N'AQAAAAEAACcQAAAAEAKSaeJb61E8Ejg4XLY9opgsHRFZXE2X6eSG+V/K4hzNxGXZSU1KCvigMqQcba7IbA==', N'24ENQDEUYPEVRFF3SPYGAGQIWGIJT4OR', N'eab08a6c-fc5d-4f9d-a754-2056eee7c9a0', NULL, 0, NULL, 1, 0)\r\n");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id='5ee30507-dc59-45b6-900e-2a5ad0f5a026'");
        }
    }
}
