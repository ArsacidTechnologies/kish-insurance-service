using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kish_insurance_service.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCoverageTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coverages_CoverageTypes_Type",
                table: "Coverages");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Coverages",
                newName: "CoverageTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Coverages_Type",
                table: "Coverages",
                newName: "IX_Coverages_CoverageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coverages_CoverageTypes_CoverageTypeId",
                table: "Coverages",
                column: "CoverageTypeId",
                principalTable: "CoverageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coverages_CoverageTypes_CoverageTypeId",
                table: "Coverages");

            migrationBuilder.RenameColumn(
                name: "CoverageTypeId",
                table: "Coverages",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_Coverages_CoverageTypeId",
                table: "Coverages",
                newName: "IX_Coverages_Type");

            migrationBuilder.AddForeignKey(
                name: "FK_Coverages_CoverageTypes_Type",
                table: "Coverages",
                column: "Type",
                principalTable: "CoverageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
