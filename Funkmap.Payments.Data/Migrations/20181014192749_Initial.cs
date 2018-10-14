using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
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
                table: "ProductLocales",
                columns: new[] { "Id", "Currency", "Description", "Language", "Name", "ProductName", "Total" },
                values: new object[] { 1L, "RUB", "Предоставляет возможность создания более 1 профиля. Применятеся к пользователю ресурса (не к его профилям).", "ru", "Pro-аккаунт", "pro_account", 300m });

            migrationBuilder.InsertData(
                table: "ProductLocales",
                columns: new[] { "Id", "Currency", "Description", "Language", "Name", "ProductName", "Total" },
                values: new object[] { 2L, "USD", "Provides the ability to create more than 1 profile. Apply to the resource user (not to his profiles).", "en", "Pro-account", "pro_account", 5m });

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
