using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Data.Migrations
{
    public partial class task_table_updated_with_status_foreignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "EmpTasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_EmpTasks_Status",
                table: "EmpTasks",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_EmpTasks_OptionTypes_Status",
                table: "EmpTasks",
                column: "Status",
                principalTable: "OptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmpTasks_OptionTypes_Status",
                table: "EmpTasks");

            migrationBuilder.DropIndex(
                name: "IX_EmpTasks_Status",
                table: "EmpTasks");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "EmpTasks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
