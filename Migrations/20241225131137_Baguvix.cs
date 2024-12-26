using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class Baguvix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_ExecutedByUserId",
                table: "Audits");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AspNetUsers_ExecutedByUserId",
                table: "Audits",
                column: "ExecutedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_ExecutedByUserId",
                table: "Audits");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AspNetUsers_ExecutedByUserId",
                table: "Audits",
                column: "ExecutedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
