using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace News.Api.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Headline = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailFrequency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubscriptions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionArticles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionArticles_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionArticles_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00b8c8aa-bc4a-4ce6-b06b-5eba48a5701b"), new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7217), "Science news and discoveries", "science", new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7217) },
                    { new Guid("12b1061f-00a4-4ac8-8b5f-2fa95e1cd0c0"), new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7215), "Health and wellness news", "health", new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7215) },
                    { new Guid("5bce9273-1b44-43c2-be94-042861f5440b"), new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7207), "Entertainment and celebrity news", "entertainment", new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7207) },
                    { new Guid("6e2c6859-8d25-4596-8665-ec0f6243609f"), new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7222), "Latest technology news and updates", "technology", new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7223) },
                    { new Guid("ade80a31-16ee-4dd1-9aa5-95279f2f218d"), new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7220), "Sports news and updates", "sports", new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7220) },
                    { new Guid("b76ecde9-d202-4434-9931-6fa01db605b3"), new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7212), "General news and top stories", "general", new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7212) },
                    { new Guid("e870c5d1-60ba-4285-a5e5-8730c77d7525"), new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7203), "Business news, finance, and market updates", "business", new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7204) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "Password", "Role", "UpdatedAt", "Username" },
                values: new object[] { new Guid("0a966293-91e7-4ff1-a08e-ad4b4cd94f94"), new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7398), "admin@newsapi.com", "Admin", "User", "Admin123!", "Admin", new DateTime(2025, 9, 1, 17, 6, 40, 141, DateTimeKind.Utc).AddTicks(7399), "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionArticles_ArticleId",
                table: "CollectionArticles",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionArticles_CollectionId_ArticleId",
                table: "CollectionArticles",
                columns: new[] { "CollectionId", "ArticleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collections_UserId",
                table: "Collections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_CategoryId",
                table: "UserSubscriptions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_UserId_CategoryId",
                table: "UserSubscriptions",
                columns: new[] { "UserId", "CategoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectionArticles");

            migrationBuilder.DropTable(
                name: "UserSubscriptions");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
