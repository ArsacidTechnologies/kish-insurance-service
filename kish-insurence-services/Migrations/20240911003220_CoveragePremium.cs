using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kish_insurance_service.Migrations
{
    /// <inheritdoc />
    public partial class CoveragePremium : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Premium",
                table: "Coverages",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Premium",
                table: "Coverages");
        }
    }
}
