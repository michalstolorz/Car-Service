using Microsoft.EntityFrameworkCore.Migrations;

namespace CarServices.Migrations
{
    public partial class UsedRepairTypeADD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repair_RepairType_TypeId",
                table: "Repair");

            migrationBuilder.DropIndex(
                name: "IX_Repair_TypeId",
                table: "Repair");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Repair");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RepairType",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UsedRepairType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairId = table.Column<int>(nullable: false),
                    RepairTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedRepairType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsedRepairType_Repair_RepairId",
                        column: x => x.RepairId,
                        principalTable: "Repair",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsedRepairType_RepairType_RepairTypeId",
                        column: x => x.RepairTypeId,
                        principalTable: "RepairType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsedRepairType_RepairId",
                table: "UsedRepairType",
                column: "RepairId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedRepairType_RepairTypeId",
                table: "UsedRepairType",
                column: "RepairTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsedRepairType");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RepairType",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Repair",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Repair_TypeId",
                table: "Repair",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repair_RepairType_TypeId",
                table: "Repair",
                column: "TypeId",
                principalTable: "RepairType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
