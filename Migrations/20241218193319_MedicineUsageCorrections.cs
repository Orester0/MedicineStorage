using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class MedicineUsageCorrections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_ClosedByUserId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_CreatedByUserId",
                table: "Audits");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Audits",
                newName: "PlannedByUserId");

            migrationBuilder.RenameColumn(
                name: "ClosedByUserId",
                table: "Audits",
                newName: "ExecutedByUserId");

            migrationBuilder.RenameColumn(
                name: "AuditDate",
                table: "Audits",
                newName: "PlannedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Audits_CreatedByUserId",
                table: "Audits",
                newName: "IX_Audits_PlannedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Audits_ClosedByUserId",
                table: "Audits",
                newName: "IX_Audits_ExecutedByUserId");

            migrationBuilder.AddColumn<int>(
                name: "MedicineRequestId",
                table: "MedicineUsages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Audits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Audits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicineUsages_MedicineRequestId",
                table: "MedicineUsages",
                column: "MedicineRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AspNetUsers_ExecutedByUserId",
                table: "Audits",
                column: "ExecutedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AspNetUsers_PlannedByUserId",
                table: "Audits",
                column: "PlannedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineUsages_MedicineRequests_MedicineRequestId",
                table: "MedicineUsages",
                column: "MedicineRequestId",
                principalTable: "MedicineRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_ExecutedByUserId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_PlannedByUserId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineUsages_MedicineRequests_MedicineRequestId",
                table: "MedicineUsages");

            migrationBuilder.DropIndex(
                name: "IX_MedicineUsages_MedicineRequestId",
                table: "MedicineUsages");

            migrationBuilder.DropColumn(
                name: "MedicineRequestId",
                table: "MedicineUsages");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Audits");

            migrationBuilder.RenameColumn(
                name: "PlannedDate",
                table: "Audits",
                newName: "AuditDate");

            migrationBuilder.RenameColumn(
                name: "PlannedByUserId",
                table: "Audits",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "ExecutedByUserId",
                table: "Audits",
                newName: "ClosedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Audits_PlannedByUserId",
                table: "Audits",
                newName: "IX_Audits_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Audits_ExecutedByUserId",
                table: "Audits",
                newName: "IX_Audits_ClosedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AspNetUsers_ClosedByUserId",
                table: "Audits",
                column: "ClosedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AspNetUsers_CreatedByUserId",
                table: "Audits",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
