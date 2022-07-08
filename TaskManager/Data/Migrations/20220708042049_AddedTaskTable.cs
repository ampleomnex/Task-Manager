using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Data.Migrations
{
    public partial class AddedTaskTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ETasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriorityID = table.Column<int>(type: "int", nullable: false),
                    EstTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    EpicsID = table.Column<int>(type: "int", nullable: false),
                    AssignedTo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ETasks_AspNetUsers_AssignedTo",
                        column: x => x.AssignedTo,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ETasks_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ETasks_AspNetUsers_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ETasks_Epics_EpicsID",
                        column: x => x.EpicsID,
                        principalTable: "Epics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ETasks_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ETasks_AssignedTo",
                table: "ETasks",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_ETasks_CreatedBy",
                table: "ETasks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ETasks_EpicsID",
                table: "ETasks",
                column: "EpicsID");

            migrationBuilder.CreateIndex(
                name: "IX_ETasks_ProjectID",
                table: "ETasks",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ETasks_RequestedBy",
                table: "ETasks",
                column: "RequestedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ETasks");
            
        }
    }
}
