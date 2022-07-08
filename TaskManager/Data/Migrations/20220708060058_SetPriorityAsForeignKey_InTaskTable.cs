using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Data.Migrations
{
    public partial class SetPriorityAsForeignKey_InTaskTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ETasks_PriorityID",
                table: "ETasks",
                column: "PriorityID");

            migrationBuilder.AddForeignKey(
                name: "FK_ETasks_OptionTypes_PriorityID",
                table: "ETasks",
                column: "PriorityID",
                principalTable: "OptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ETasks_OptionTypes_PriorityID",
                table: "ETasks");

            migrationBuilder.DropIndex(
                name: "IX_ETasks_PriorityID",
                table: "ETasks");
        }
    }
}
