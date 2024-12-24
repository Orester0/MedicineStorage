using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class ObjectCycleRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenderProposals_Tenders_TenderId",
                table: "TenderProposals");

            migrationBuilder.AddForeignKey(
                name: "FK_TenderProposals_Tenders_TenderId",
                table: "TenderProposals",
                column: "TenderId",
                principalTable: "Tenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenderProposals_Tenders_TenderId",
                table: "TenderProposals");

            migrationBuilder.AddForeignKey(
                name: "FK_TenderProposals_Tenders_TenderId",
                table: "TenderProposals",
                column: "TenderId",
                principalTable: "Tenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
