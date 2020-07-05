using Microsoft.EntityFrameworkCore.Migrations;

namespace CarServices.Migrations
{
    public partial class RepairStatusForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Repair_StatusId",
                table: "Repair",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repair_RepairStatus_StatusId",
                table: "Repair",
                column: "StatusId",
                principalTable: "RepairStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repair_RepairStatus_StatusId",
                table: "Repair");

            migrationBuilder.DropIndex(
                name: "IX_Repair_StatusId",
                table: "Repair");
        }
    }
}
