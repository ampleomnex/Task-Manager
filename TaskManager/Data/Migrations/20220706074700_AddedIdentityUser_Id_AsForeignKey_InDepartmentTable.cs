using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Data.Migrations
{
    public partial class AddedIdentityUser_Id_AsForeignKey_InDepartmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Departments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UserId",
                table: "Departments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_UserId",
                table: "Departments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_UserId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_UserId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Departments");
        }
    }
}
