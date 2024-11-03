using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class AllModelChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditItems_Medicines_MedicineId",
                table: "AuditItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Audits_Users_AuditorId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_Audits_Users_DoctorId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineRequests_Users_ApproverId",
                table: "MedicineRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineRequests_Users_RequesterId",
                table: "MedicineRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineUsages_Medicines_MedicineId",
                table: "MedicineUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineUsages_Users_UserId",
                table: "MedicineUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_DistributorBids_WinningBidId",
                table: "Tenders");

            migrationBuilder.DropTable(
                name: "BidItems");

            migrationBuilder.DropTable(
                name: "MedicineRequestItems");

            migrationBuilder.DropTable(
                name: "RequestApprovals");

            migrationBuilder.DropTable(
                name: "StockAdjustments");

            migrationBuilder.DropTable(
                name: "TenderRequests");

            migrationBuilder.DropTable(
                name: "DistributorBids");

            migrationBuilder.DropTable(
                name: "Distributors");

            migrationBuilder.DropIndex(
                name: "IX_Tenders_WinningBidId",
                table: "Tenders");

            migrationBuilder.DropIndex(
                name: "IX_MedicineRequests_ApproverId",
                table: "MedicineRequests");

            migrationBuilder.DropIndex(
                name: "IX_Audits_AuditorId",
                table: "Audits");

            migrationBuilder.DropIndex(
                name: "IX_Audits_DoctorId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "WinningBidId",
                table: "Tenders");

            migrationBuilder.DropColumn(
                name: "CurrentStock",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "IsControlled",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "StorageRequirements",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "MedicineRequests");

            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "MedicineRequests");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "MedicineRequests");

            migrationBuilder.DropColumn(
                name: "RequiredForDate",
                table: "MedicineRequests");

            migrationBuilder.DropColumn(
                name: "AuditorId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Findings",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "ActualStock",
                table: "AuditItems");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "AuditItems");

            migrationBuilder.DropColumn(
                name: "SystemStock",
                table: "AuditItems");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Tenders",
                newName: "PublishDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Tenders",
                newName: "DeadlineDate");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MedicineUsages",
                newName: "UsedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicineUsages_UserId",
                table: "MedicineUsages",
                newName: "IX_MedicineUsages_UsedByUserId");

            migrationBuilder.RenameColumn(
                name: "RequesterId",
                table: "MedicineRequests",
                newName: "RequestedByUserId");

            migrationBuilder.RenameColumn(
                name: "RecurringIntervalDays",
                table: "MedicineRequests",
                newName: "ApprovedByUserId");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "MedicineRequests",
                newName: "RequestType");

            migrationBuilder.RenameIndex(
                name: "IX_MedicineRequests_RequesterId",
                table: "MedicineRequests",
                newName: "IX_MedicineRequests_RequestedByUserId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Audits",
                newName: "ConductedByUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tenders",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Tenders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Tenders",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "MedicineUsages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "MedicineUsages",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Medicines",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Medicines",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Medicines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "MedicineRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Justification",
                table: "MedicineRequests",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MedicineId",
                table: "MedicineRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "MedicineRequests",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequiredByDate",
                table: "MedicineRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Audits",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Discrepancy",
                table: "AuditItems",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "ActualQuantity",
                table: "AuditItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedQuantity",
                table: "AuditItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "TenderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenderId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    RequiredQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenderItems_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TenderItems_Tenders_TenderId",
                        column: x => x.TenderId,
                        principalTable: "Tenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenderProposals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenderId = table.Column<int>(type: "int", nullable: false),
                    DistributorId = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenderProposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenderProposals_Tenders_TenderId",
                        column: x => x.TenderId,
                        principalTable: "Tenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TenderProposals_Users_DistributorId",
                        column: x => x.DistributorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenderProposalItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenderProposalId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenderProposalItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenderProposalItems_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TenderProposalItems_TenderProposals_TenderProposalId",
                        column: x => x.TenderProposalId,
                        principalTable: "TenderProposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_CreatedByUserId",
                table: "Tenders",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineRequests_ApprovedByUserId",
                table: "MedicineRequests",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineRequests_MedicineId",
                table: "MedicineRequests",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_ConductedByUserId",
                table: "Audits",
                column: "ConductedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_BatchNumber",
                table: "Stocks",
                column: "BatchNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_MedicineId",
                table: "Stocks",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_TenderItems_MedicineId",
                table: "TenderItems",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_TenderItems_TenderId",
                table: "TenderItems",
                column: "TenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TenderProposalItems_MedicineId",
                table: "TenderProposalItems",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_TenderProposalItems_TenderProposalId",
                table: "TenderProposalItems",
                column: "TenderProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_TenderProposals_DistributorId",
                table: "TenderProposals",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_TenderProposals_TenderId",
                table: "TenderProposals",
                column: "TenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditItems_Medicines_MedicineId",
                table: "AuditItems",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_Users_ConductedByUserId",
                table: "Audits",
                column: "ConductedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineRequests_Medicines_MedicineId",
                table: "MedicineRequests",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineRequests_Users_ApprovedByUserId",
                table: "MedicineRequests",
                column: "ApprovedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineRequests_Users_RequestedByUserId",
                table: "MedicineRequests",
                column: "RequestedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineUsages_Medicines_MedicineId",
                table: "MedicineUsages",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineUsages_Users_UsedByUserId",
                table: "MedicineUsages",
                column: "UsedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Users_CreatedByUserId",
                table: "Tenders",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditItems_Medicines_MedicineId",
                table: "AuditItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Audits_Users_ConductedByUserId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineRequests_Medicines_MedicineId",
                table: "MedicineRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineRequests_Users_ApprovedByUserId",
                table: "MedicineRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineRequests_Users_RequestedByUserId",
                table: "MedicineRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineUsages_Medicines_MedicineId",
                table: "MedicineUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineUsages_Users_UsedByUserId",
                table: "MedicineUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Users_CreatedByUserId",
                table: "Tenders");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "TenderItems");

            migrationBuilder.DropTable(
                name: "TenderProposalItems");

            migrationBuilder.DropTable(
                name: "TenderProposals");

            migrationBuilder.DropIndex(
                name: "IX_Users_Login",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tenders_CreatedByUserId",
                table: "Tenders");

            migrationBuilder.DropIndex(
                name: "IX_MedicineRequests_ApprovedByUserId",
                table: "MedicineRequests");

            migrationBuilder.DropIndex(
                name: "IX_MedicineRequests_MedicineId",
                table: "MedicineRequests");

            migrationBuilder.DropIndex(
                name: "IX_Audits_ConductedByUserId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Tenders");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Tenders");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "MedicineRequests");

            migrationBuilder.DropColumn(
                name: "Justification",
                table: "MedicineRequests");

            migrationBuilder.DropColumn(
                name: "MedicineId",
                table: "MedicineRequests");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "MedicineRequests");

            migrationBuilder.DropColumn(
                name: "RequiredByDate",
                table: "MedicineRequests");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "ActualQuantity",
                table: "AuditItems");

            migrationBuilder.DropColumn(
                name: "ExpectedQuantity",
                table: "AuditItems");

            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "Tenders",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "DeadlineDate",
                table: "Tenders",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "UsedByUserId",
                table: "MedicineUsages",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicineUsages_UsedByUserId",
                table: "MedicineUsages",
                newName: "IX_MedicineUsages_UserId");

            migrationBuilder.RenameColumn(
                name: "RequestedByUserId",
                table: "MedicineRequests",
                newName: "RequesterId");

            migrationBuilder.RenameColumn(
                name: "RequestType",
                table: "MedicineRequests",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "ApprovedByUserId",
                table: "MedicineRequests",
                newName: "RecurringIntervalDays");

            migrationBuilder.RenameIndex(
                name: "IX_MedicineRequests_RequestedByUserId",
                table: "MedicineRequests",
                newName: "IX_MedicineRequests_RequesterId");

            migrationBuilder.RenameColumn(
                name: "ConductedByUserId",
                table: "Audits",
                newName: "Type");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tenders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<int>(
                name: "WinningBidId",
                table: "Tenders",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "MedicineUsages",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "MedicineUsages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Medicines",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Medicines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentStock",
                table: "Medicines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Medicines",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Medicines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsControlled",
                table: "Medicines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StorageRequirements",
                table: "Medicines",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ApproverId",
                table: "MedicineRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "MedicineRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "MedicineRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RequiredForDate",
                table: "MedicineRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AuditorId",
                table: "Audits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Findings",
                table: "Audits",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discrepancy",
                table: "AuditItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<decimal>(
                name: "ActualStock",
                table: "AuditItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "AuditItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "SystemStock",
                table: "AuditItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Distributors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicineRequestItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Justification = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineRequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineRequestItems_MedicineRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "MedicineRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicineRequestItems_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApproverId = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestApprovals_MedicineRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "MedicineRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestApprovals_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AdjustmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenderRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    TenderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenderRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenderRequests_MedicineRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "MedicineRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenderRequests_Tenders_TenderId",
                        column: x => x.TenderId,
                        principalTable: "Tenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DistributorBids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistributorId = table.Column<int>(type: "int", nullable: false),
                    TenderId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributorBids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistributorBids_Distributors_DistributorId",
                        column: x => x.DistributorId,
                        principalTable: "Distributors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributorBids_Tenders_TenderId",
                        column: x => x.TenderId,
                        principalTable: "Tenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BidItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BidItems_DistributorBids_BidId",
                        column: x => x.BidId,
                        principalTable: "DistributorBids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BidItems_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_WinningBidId",
                table: "Tenders",
                column: "WinningBidId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineRequests_ApproverId",
                table: "MedicineRequests",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_AuditorId",
                table: "Audits",
                column: "AuditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_DoctorId",
                table: "Audits",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_BidItems_BidId",
                table: "BidItems",
                column: "BidId");

            migrationBuilder.CreateIndex(
                name: "IX_BidItems_MedicineId",
                table: "BidItems",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributorBids_DistributorId",
                table: "DistributorBids",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributorBids_TenderId",
                table: "DistributorBids",
                column: "TenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_Email",
                table: "Distributors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_Name",
                table: "Distributors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicineRequestItems_MedicineId",
                table: "MedicineRequestItems",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineRequestItems_RequestId",
                table: "MedicineRequestItems",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestApprovals_ApproverId",
                table: "RequestApprovals",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestApprovals_RequestId",
                table: "RequestApprovals",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_MedicineId",
                table: "StockAdjustments",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_UserId",
                table: "StockAdjustments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TenderRequests_RequestId",
                table: "TenderRequests",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TenderRequests_TenderId",
                table: "TenderRequests",
                column: "TenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditItems_Medicines_MedicineId",
                table: "AuditItems",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_Users_AuditorId",
                table: "Audits",
                column: "AuditorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_Users_DoctorId",
                table: "Audits",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineRequests_Users_ApproverId",
                table: "MedicineRequests",
                column: "ApproverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineRequests_Users_RequesterId",
                table: "MedicineRequests",
                column: "RequesterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineUsages_Medicines_MedicineId",
                table: "MedicineUsages",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineUsages_Users_UserId",
                table: "MedicineUsages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_DistributorBids_WinningBidId",
                table: "Tenders",
                column: "WinningBidId",
                principalTable: "DistributorBids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
