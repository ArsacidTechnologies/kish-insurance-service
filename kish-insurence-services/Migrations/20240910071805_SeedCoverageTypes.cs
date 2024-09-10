using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace kish_insurance_service.Migrations
{
    /// <inheritdoc />
    public partial class SeedCoverageTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CoverageTypes",
                columns: new[] { "Id", "MaxCapital", "MinCapital", "Name", "PremiumRate" },
                values: new object[,]
                {
                    { 1, 500000000m, 5000m, "Surgery", 0.0052m },
                    { 2, 400000000m, 4000m, "Dental", 0.0042m },
                    { 3, 200000000m, 2000m, "Hospitalization", 0.005m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CoverageTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CoverageTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CoverageTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
