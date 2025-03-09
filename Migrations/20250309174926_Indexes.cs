using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tenders_Status",
                table: "Tenders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_Title",
                table: "Tenders",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Title",
                table: "Notifications",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_Category",
                table: "Medicines",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_Status",
                table: "Audits",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_Title",
                table: "Audits",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FirstName_LastName",
                table: "AspNetUsers",
                columns: new[] { "FirstName", "LastName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenders_Status",
                table: "Tenders");

            migrationBuilder.DropIndex(
                name: "IX_Tenders_Title",
                table: "Tenders");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_Title",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_Category",
                table: "Medicines");

            migrationBuilder.DropIndex(
                name: "IX_Audits_Status",
                table: "Audits");

            migrationBuilder.DropIndex(
                name: "IX_Audits_Title",
                table: "Audits");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FirstName_LastName",
                table: "AspNetUsers");
        }
    }
}
