using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class ObjectCycleDetected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineRequests_Medicines_MedicineId",
                table: "MedicineRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineUsages_Medicines_MedicineId",
                table: "MedicineUsages");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineRequests_Medicines_MedicineId",
                table: "MedicineRequests",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineUsages_Medicines_MedicineId",
                table: "MedicineUsages",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineRequests_Medicines_MedicineId",
                table: "MedicineRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineUsages_Medicines_MedicineId",
                table: "MedicineUsages");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineRequests_Medicines_MedicineId",
                table: "MedicineRequests",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineUsages_Medicines_MedicineId",
                table: "MedicineUsages",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
