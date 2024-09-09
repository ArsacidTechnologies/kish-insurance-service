using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kish_insurance_service.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoverageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PremiumRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinCapital = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxCapital = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coverages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Capital = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsuranceRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coverages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coverages_CoverageTypes_Type",
                        column: x => x.Type,
                        principalTable: "CoverageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Coverages_InsuranceRequests_InsuranceRequestId",
                        column: x => x.InsuranceRequestId,
                        principalTable: "InsuranceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coverages_InsuranceRequestId",
                table: "Coverages",
                column: "InsuranceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Coverages_Type",
                table: "Coverages",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coverages");

            migrationBuilder.DropTable(
                name: "CoverageTypes");

            migrationBuilder.DropTable(
                name: "InsuranceRequests");
        }
    }
}
