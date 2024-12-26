using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class MakeExecuteUserNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_ExecutedByUserId",
                table: "Audits");

            migrationBuilder.AlterColumn<int>(
                name: "ExecutedByUserId",
                table: "Audits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AspNetUsers_ExecutedByUserId",
                table: "Audits",
                column: "ExecutedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_ExecutedByUserId",
                table: "Audits");

            migrationBuilder.AlterColumn<int>(
                name: "ExecutedByUserId",
                table: "Audits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AspNetUsers_ExecutedByUserId",
                table: "Audits",
                column: "ExecutedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
