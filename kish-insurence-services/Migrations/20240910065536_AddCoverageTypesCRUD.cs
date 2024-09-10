using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kish_insurance_service.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverageTypesCRUD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PremiumRate",
                table: "CoverageTypes",
                type: "decimal(5,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PremiumRate",
                table: "CoverageTypes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)");
        }
    }
}
