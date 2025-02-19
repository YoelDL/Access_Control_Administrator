using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACA.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProcessName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supervisors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Supervisor_Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CI = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    schoolLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Operator_Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CI = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    schoolLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    AsignedSupervisorId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operators_Supervisors_AsignedSupervisorId",
                        column: x => x.AsignedSupervisorId,
                        principalTable: "Supervisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Process_Supervisor",
                columns: table => new
                {
                    SupervisorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProcessId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Process_Supervisor", x => new { x.ProcessId, x.SupervisorId });
                    table.ForeignKey(
                        name: "FK_Process_Supervisor_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Process_Supervisor_Supervisors_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Supervisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Processes_Operators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OperatorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProcessId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes_Operators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processes_Operators_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_Operators_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operators_AsignedSupervisorId",
                table: "Operators",
                column: "AsignedSupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Process_Supervisor_SupervisorId",
                table: "Process_Supervisor",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_Operators_OperatorId",
                table: "Processes_Operators",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_Operators_ProcessId",
                table: "Processes_Operators",
                column: "ProcessId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Process_Supervisor");

            migrationBuilder.DropTable(
                name: "Processes_Operators");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "Supervisors");
        }
    }
}
