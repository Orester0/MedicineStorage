using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class UniqueIndexOnMedicine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Medicines_Name",
                table: "Medicines",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Medicines_Name",
                table: "Medicines");
        }
    }
}
