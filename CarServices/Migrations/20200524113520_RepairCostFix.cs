using Microsoft.EntityFrameworkCore.Migrations;

namespace CarServices.Migrations
{
    public partial class RepairCostFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repair_Employees_EmployeesId",
                table: "Repair");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeesId",
                table: "Repair",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "Repair",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Repair_Employees_EmployeesId",
                table: "Repair",
                column: "EmployeesId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repair_Employees_EmployeesId",
                table: "Repair");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeesId",
                table: "Repair",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Cost",
                table: "Repair",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Repair_Employees_EmployeesId",
                table: "Repair",
                column: "EmployeesId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
