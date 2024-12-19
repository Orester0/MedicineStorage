using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class MedicineSupplyModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosingDate",
                table: "Tenders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TenderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenderProposalItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(10,0)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_TenderProposalItems_TenderProposalItemId",
                        column: x => x.TenderProposalItemId,
                        principalTable: "TenderProposalItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_TenderProposalItemId",
                table: "InventoryTransactions",
                column: "TenderProposalItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "ClosingDate",
                table: "Tenders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TenderItems");
        }
    }
}
