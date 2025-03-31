using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechPM_Dashboard.Migrations
{
    public partial class modelsAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProgressPercentage = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllocationPercentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Risks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    MitigationPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Risks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    AllocationPercentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceAllocations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceAllocations_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AssignedToId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Resources_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Description", "EndDate", "Name", "ProgressPercentage", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, "Upgrade company infrastructure", new DateTime(2025, 5, 29, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4799), "Infrastructure Upgrade", 65, new DateTime(2025, 1, 29, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4755), 0 },
                    { 2, "Migrate data to new system", new DateTime(2025, 6, 29, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4815), "Data Migration", 42, new DateTime(2025, 2, 28, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4813), 1 },
                    { 3, "Integrate with cloud services", new DateTime(2025, 7, 29, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4828), "Cloud Integration", 28, new DateTime(2024, 12, 29, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4824), 2 },
                    { 4, "Conduct security audit", new DateTime(2025, 4, 13, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4833), "Security Audit", 85, new DateTime(2024, 11, 29, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4831), 0 },
                    { 5, "Setup new networking infrastructure", new DateTime(2025, 4, 29, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4849), "Networking Setup", 72, new DateTime(2024, 12, 29, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4846), 0 }
                });

            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "Id", "AllocationPercentage", "Email", "FirstName", "LastName", "Role" },
                values: new object[,]
                {
                    { 1, 115, "john.doe@example.com", "John", "Doe", "Senior Developer" },
                    { 2, 85, "jane.smith@example.com", "Jane", "Smith", "DevOps Engineer" },
                    { 3, 60, "robert.johnson@example.com", "Robert", "Johnson", "Network Administrator" },
                    { 4, 40, "amanda.parker@example.com", "Amanda", "Parker", "Database Administrator" }
                });

            migrationBuilder.InsertData(
                table: "ResourceAllocations",
                columns: new[] { "Id", "AllocationPercentage", "ProjectId", "ResourceId" },
                values: new object[,]
                {
                    { 1, 30, 1, 1 },
                    { 2, 40, 2, 1 },
                    { 3, 45, 3, 1 },
                    { 4, 25, 1, 2 },
                    { 5, 60, 3, 2 },
                    { 6, 60, 1, 3 },
                    { 7, 40, 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "Risks",
                columns: new[] { "Id", "Description", "IsResolved", "Level", "MitigationPlan", "ProjectId" },
                values: new object[,]
                {
                    { 1, "Database compatibility issues", false, 2, "Test in staging environment first", 2 },
                    { 2, "Security vulnerabilities in cloud setup", false, 2, "Perform security audit", 3 },
                    { 3, "Hardware delivery delays", false, 1, "Order from alternate vendor", 1 },
                    { 4, "Staff training availability", false, 0, "Schedule training sessions in advance", 4 }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AssignedToId", "Description", "DueDate", "Name", "ProjectId", "Status" },
                values: new object[,]
                {
                    { 1, 3, "Install and configure new network switches in server room", new DateTime(2025, 4, 3, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4949), "Configure network switches", 1, 1 },
                    { 2, 4, "Transfer all user profiles and data to the upgraded database", new DateTime(2025, 3, 27, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4957), "Migrate user data to new system", 2, 3 },
                    { 3, 2, "Configure security groups and network ACLs for AWS deployment", new DateTime(2025, 3, 24, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4985), "Set up AWS security groups", 3, 2 },
                    { 4, 1, "Run security penetration tests against production environment", new DateTime(2025, 4, 8, 12, 40, 58, 125, DateTimeKind.Local).AddTicks(4990), "Conduct penetration testing", 4, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAllocations_ProjectId",
                table: "ResourceAllocations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAllocations_ResourceId",
                table: "ResourceAllocations",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_ProjectId",
                table: "Risks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedToId",
                table: "Tasks",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceAllocations");

            migrationBuilder.DropTable(
                name: "Risks");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Resources");
        }
    }
}
