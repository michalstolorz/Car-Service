using Microsoft.EntityFrameworkCore.Migrations;

namespace CarServices.Migrations
{
    public partial class DataBaseForeignKeysFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Employees",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UsedParts_PartId",
                table: "UsedParts",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedParts_RepairId",
                table: "UsedParts",
                column: "RepairId");

            migrationBuilder.CreateIndex(
                name: "IX_Repair_CarId",
                table: "Repair",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Repair_EmployeesId",
                table: "Repair",
                column: "EmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_Repair_InvoiceId",
                table: "Repair",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Repair_TypeId",
                table: "Repair",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_EmployeesId",
                table: "Order",
                column: "EmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_UserId",
                table: "Employees",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Employees_EmployeesId",
                table: "Order",
                column: "EmployeesId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Repair_Car_CarId",
                table: "Repair",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Repair_Employees_EmployeesId",
                table: "Repair",
                column: "EmployeesId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Repair_Invoice_InvoiceId",
                table: "Repair",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repair_RepairType_TypeId",
                table: "Repair",
                column: "TypeId",
                principalTable: "RepairType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedParts_Parts_PartId",
                table: "UsedParts",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedParts_Repair_RepairId",
                table: "UsedParts",
                column: "RepairId",
                principalTable: "Repair",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_UserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Employees_EmployeesId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Repair_Car_CarId",
                table: "Repair");

            migrationBuilder.DropForeignKey(
                name: "FK_Repair_Employees_EmployeesId",
                table: "Repair");

            migrationBuilder.DropForeignKey(
                name: "FK_Repair_Invoice_InvoiceId",
                table: "Repair");

            migrationBuilder.DropForeignKey(
                name: "FK_Repair_RepairType_TypeId",
                table: "Repair");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedParts_Parts_PartId",
                table: "UsedParts");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedParts_Repair_RepairId",
                table: "UsedParts");

            migrationBuilder.DropIndex(
                name: "IX_UsedParts_PartId",
                table: "UsedParts");

            migrationBuilder.DropIndex(
                name: "IX_UsedParts_RepairId",
                table: "UsedParts");

            migrationBuilder.DropIndex(
                name: "IX_Repair_CarId",
                table: "Repair");

            migrationBuilder.DropIndex(
                name: "IX_Repair_EmployeesId",
                table: "Repair");

            migrationBuilder.DropIndex(
                name: "IX_Repair_InvoiceId",
                table: "Repair");

            migrationBuilder.DropIndex(
                name: "IX_Repair_TypeId",
                table: "Repair");

            migrationBuilder.DropIndex(
                name: "IX_Order_EmployeesId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Employees",
                newName: "UserID");

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Employees",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
