using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Data.Migrations
{
    public partial class TaskTableadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmpTasks",
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
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpTasks_AspNetUsers_AssignedTo",
                        column: x => x.AssignedTo,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpTasks_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmpTasks_AspNetUsers_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmpTasks_Epics_EpicsID",
                        column: x => x.EpicsID,
                        principalTable: "Epics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmpTasks_OptionTypes_PriorityID",
                        column: x => x.PriorityID,
                        principalTable: "OptionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmpTasks_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpTasks_AssignedTo",
                table: "EmpTasks",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_EmpTasks_CreatedBy",
                table: "EmpTasks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EmpTasks_EpicsID",
                table: "EmpTasks",
                column: "EpicsID");

            migrationBuilder.CreateIndex(
                name: "IX_EmpTasks_PriorityID",
                table: "EmpTasks",
                column: "PriorityID");

            migrationBuilder.CreateIndex(
                name: "IX_EmpTasks_ProjectID",
                table: "EmpTasks",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_EmpTasks_RequestedBy",
                table: "EmpTasks",
                column: "RequestedBy");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpTasks");         

            
        }
    }
}
