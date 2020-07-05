using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarServices.Migrations
{
    public partial class RepairStautsADD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Status",
            //    table: "Repair");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCompletion",
                table: "Repair",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Repair",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Repair",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RepairStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairStatus", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepairStatus");

            migrationBuilder.DropColumn(
                name: "DateOfCompletion",
                table: "Repair");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Repair");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Repair");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Repair",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
