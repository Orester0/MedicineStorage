using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class MedicineStockRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_ConductedByUserId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_UserId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineRequests_AspNetUsers_UserId",
                table: "MedicineRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TenderProposals_AspNetUsers_DistributorId",
                table: "TenderProposals");

            migrationBuilder.DropIndex(
                name: "IX_MedicineRequests_UserId",
                table: "MedicineRequests");

            migrationBuilder.DropIndex(
                name: "IX_Audits_UserId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "MedicineUsages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MedicineRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Audits");

            migrationBuilder.RenameColumn(
                name: "DistributorId",
                table: "TenderProposals",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TenderProposals_DistributorId",
                table: "TenderProposals",
                newName: "IX_TenderProposals_CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "ConductedByUserId",
                table: "Audits",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Audits_ConductedByUserId",
                table: "Audits",
                newName: "IX_Audits_CreatedByUserId");

            migrationBuilder.AddColumn<int>(
                name: "ClosedByUserId",
                table: "Tenders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OpenedByUserId",
                table: "Tenders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WinnerSelectedByUserId",
                table: "Tenders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClosedByUserId",
                table: "Audits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_ClosedByUserId",
                table: "Tenders",
                column: "ClosedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_OpenedByUserId",
                table: "Tenders",
                column: "OpenedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_WinnerSelectedByUserId",
                table: "Tenders",
                column: "WinnerSelectedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_ClosedByUserId",
                table: "Audits",
                column: "ClosedByUserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_TenderProposals_AspNetUsers_CreatedByUserId",
                table: "TenderProposals",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_AspNetUsers_ClosedByUserId",
                table: "Tenders",
                column: "ClosedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_AspNetUsers_OpenedByUserId",
                table: "Tenders",
                column: "OpenedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_AspNetUsers_WinnerSelectedByUserId",
                table: "Tenders",
                column: "WinnerSelectedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_ClosedByUserId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_CreatedByUserId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_TenderProposals_AspNetUsers_CreatedByUserId",
                table: "TenderProposals");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_AspNetUsers_ClosedByUserId",
                table: "Tenders");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_AspNetUsers_OpenedByUserId",
                table: "Tenders");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_AspNetUsers_WinnerSelectedByUserId",
                table: "Tenders");

            migrationBuilder.DropIndex(
                name: "IX_Tenders_ClosedByUserId",
                table: "Tenders");

            migrationBuilder.DropIndex(
                name: "IX_Tenders_OpenedByUserId",
                table: "Tenders");

            migrationBuilder.DropIndex(
                name: "IX_Tenders_WinnerSelectedByUserId",
                table: "Tenders");

            migrationBuilder.DropIndex(
                name: "IX_Audits_ClosedByUserId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "ClosedByUserId",
                table: "Tenders");

            migrationBuilder.DropColumn(
                name: "OpenedByUserId",
                table: "Tenders");

            migrationBuilder.DropColumn(
                name: "WinnerSelectedByUserId",
                table: "Tenders");

            migrationBuilder.DropColumn(
                name: "ClosedByUserId",
                table: "Audits");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "TenderProposals",
                newName: "DistributorId");

            migrationBuilder.RenameIndex(
                name: "IX_TenderProposals_CreatedByUserId",
                table: "TenderProposals",
                newName: "IX_TenderProposals_DistributorId");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Audits",
                newName: "ConductedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Audits_CreatedByUserId",
                table: "Audits",
                newName: "IX_Audits_ConductedByUserId");

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "MedicineUsages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MedicineRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicineRequests_UserId",
                table: "MedicineRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_UserId",
                table: "Audits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AspNetUsers_ConductedByUserId",
                table: "Audits",
                column: "ConductedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AspNetUsers_UserId",
                table: "Audits",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineRequests_AspNetUsers_UserId",
                table: "MedicineRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenderProposals_AspNetUsers_DistributorId",
                table: "TenderProposals",
                column: "DistributorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
