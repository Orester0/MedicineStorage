using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineStorage.Migrations
{
    /// <inheritdoc />
    public partial class AttributesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "MedicineUsages",
                type: "decimal(18,2)",
                precision: 10,
                scale: 0,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,0)",
                oldPrecision: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "MedicineUsages",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Stock",
                table: "Medicines",
                type: "decimal(18,2)",
                precision: 12,
                scale: 0,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumStock",
                table: "Medicines",
                type: "decimal(18,2)",
                precision: 12,
                scale: 0,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)",
                oldPrecision: 12);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Medicines",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "MedicineRequests",
                type: "decimal(18,2)",
                precision: 10,
                scale: 0,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,0)",
                oldPrecision: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Justification",
                table: "MedicineRequests",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Audits",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedQuantity",
                table: "AuditItems",
                type: "decimal(18,2)",
                precision: 10,
                scale: 0,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,0)",
                oldPrecision: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualQuantity",
                table: "AuditItems",
                type: "decimal(18,2)",
                precision: 10,
                scale: 0,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,0)",
                oldPrecision: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "MedicineUsages",
                type: "decimal(10,0)",
                precision: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 10,
                oldScale: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "MedicineUsages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Stock",
                table: "Medicines",
                type: "decimal(12,0)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 12,
                oldScale: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumStock",
                table: "Medicines",
                type: "decimal(12,0)",
                precision: 12,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 12,
                oldScale: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Medicines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "MedicineRequests",
                type: "decimal(10,0)",
                precision: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 10,
                oldScale: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Justification",
                table: "MedicineRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedQuantity",
                table: "AuditItems",
                type: "decimal(10,0)",
                precision: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 10,
                oldScale: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualQuantity",
                table: "AuditItems",
                type: "decimal(10,0)",
                precision: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 10,
                oldScale: 0);
        }
    }
}
