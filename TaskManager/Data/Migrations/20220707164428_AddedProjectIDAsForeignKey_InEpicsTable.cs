using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Data.Migrations
{
    public partial class AddedProjectIDAsForeignKey_InEpicsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "Epics");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Epics",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProjectID",
                table: "Epics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Epics_CreatedBy",
                table: "Epics",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Epics_ProjectID",
                table: "Epics",
                column: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_Epics_AspNetUsers_CreatedBy",
                table: "Epics",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Epics_Projects_ProjectID",
                table: "Epics",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Epics_AspNetUsers_CreatedBy",
                table: "Epics");

            migrationBuilder.DropForeignKey(
                name: "FK_Epics_Projects_ProjectID",
                table: "Epics");

            migrationBuilder.DropIndex(
                name: "IX_Epics_CreatedBy",
                table: "Epics");

            migrationBuilder.DropIndex(
                name: "IX_Epics_ProjectID",
                table: "Epics");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "Epics");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Epics",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "Epics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
