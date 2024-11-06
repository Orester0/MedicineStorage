using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class StockRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineUsages_Stocks_StockId",
                table: "MedicineUsages");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_MedicineUsages_StockId",
                table: "MedicineUsages");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumStock",
                table: "Medicines",
                type: "decimal(12,0)",
                precision: 12,
                scale: 0,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)",
                oldPrecision: 10,
                oldScale: 4);

            migrationBuilder.AddColumn<decimal>(
                name: "Stock",
                table: "Medicines",
                type: "decimal(12,0)",
                precision: 12,
                scale: 0,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Medicines");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumStock",
                table: "Medicines",
                type: "decimal(10,4)",
                precision: 10,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)",
                oldPrecision: 12,
                oldScale: 0);

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(10,0)", precision: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicineUsages_StockId",
                table: "MedicineUsages",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_MedicineId",
                table: "Stocks",
                column: "MedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineUsages_Stocks_StockId",
                table: "MedicineUsages",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
