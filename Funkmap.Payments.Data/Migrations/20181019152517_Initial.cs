using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Funkmap.Payments.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateUtc = table.Column<DateTime>(nullable: false),
                    Currency = table.Column<string>(nullable: false),
                    Total = table.Column<decimal>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Period = table.Column<int>(nullable: false),
                    PaymentType = table.Column<int>(nullable: false),
                    HasTrial = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "ProductLocales",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Language = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Total = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLocales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductLocales_Products_ProductName",
                        column: x => x.ProductName,
                        principalTable: "Products",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Name", "HasTrial", "PaymentType", "Period" },
                values: new object[] { "pro_account", false, 1, 1 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Name", "HasTrial", "PaymentType", "Period" },
                values: new object[] { "priority_profile", false, 1, 1 });

            migrationBuilder.InsertData(
                table: "ProductLocales",
                columns: new[] { "Id", "Currency", "Description", "Language", "Name", "ProductName", "Total" },
                values: new object[,]
                {
                    { 1L, "RUB", "Предоставляет возможность создания более 1 профиля. Применятеся к пользователю ресурса (не к его профилям).", "ru-RU", "Pro-аккаунт", "pro_account", 300m },
                    { 2L, "USD", "Provides the ability to create more than 1 profile. Apply to the resource user (not to his profiles).", "en-US", "Pro-account", "pro_account", 5m },
                    { 3L, "RUB", "Позволяет выделять один из ваших профилей на карте Bandmap и выдавать одним из первых в поисковых запросах.", "ru-RU", "Приоритетный профиль", "priority_profile", 300m },
                    { 4L, "USD", "Allows you to highlight one of your profiles on the Bandmap map and issue one of the first in search queries.", "en-US", "Priority profile", "priority_profile", 5m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductLocales_ProductName",
                table: "ProductLocales",
                column: "ProductName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "ProductLocales");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
