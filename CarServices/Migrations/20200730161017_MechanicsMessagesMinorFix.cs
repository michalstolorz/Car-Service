using Microsoft.EntityFrameworkCore.Migrations;

namespace CarServices.Migrations
{
    public partial class MechanicsMessagesMinorFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "MechanicsMessages",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MechanicsMessages_EmployeeId",
                table: "MechanicsMessages",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicsMessages_Employees_EmployeeId",
                table: "MechanicsMessages",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MechanicsMessages_Employees_EmployeeId",
                table: "MechanicsMessages");

            migrationBuilder.DropIndex(
                name: "IX_MechanicsMessages_EmployeeId",
                table: "MechanicsMessages");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "MechanicsMessages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
