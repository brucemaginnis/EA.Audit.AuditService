using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EA.Audit.AuditService.Migrations
{
    public partial class InitialPostCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditApplications",
                columns: table => new
                {
                    ApplicationID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditApplications", x => x.ApplicationID);
                });

            migrationBuilder.CreateTable(
                name: "AuditLevels",
                columns: table => new
                {
                    AuditLevelID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLevels", x => x.AuditLevelID);
                });

            migrationBuilder.CreateTable(
                name: "AuditTypes",
                columns: table => new
                {
                    AuditTypeID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTypes", x => x.AuditTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ClientRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    AuditID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    ApplicationId = table.Column<int>(nullable: false),
                    AuditApplicationId = table.Column<int>(nullable: true),
                    AuditTypeId = table.Column<int>(nullable: false),
                    Source = table.Column<string>(nullable: true),
                    AuditLevelId = table.Column<int>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.AuditID);
                    table.ForeignKey(
                        name: "FK_Audits_AuditApplications_AuditApplicationId",
                        column: x => x.AuditApplicationId,
                        principalTable: "AuditApplications",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Audits_AuditLevels_AuditLevelId",
                        column: x => x.AuditLevelId,
                        principalTable: "AuditLevels",
                        principalColumn: "AuditLevelID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Audits_AuditTypes_AuditTypeId",
                        column: x => x.AuditTypeId,
                        principalTable: "AuditTypes",
                        principalColumn: "AuditTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audits_AuditApplicationId",
                table: "Audits",
                column: "AuditApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_AuditLevelId",
                table: "Audits",
                column: "AuditLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_AuditTypeId",
                table: "Audits",
                column: "AuditTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DropTable(
                name: "ClientRequests");

            migrationBuilder.DropTable(
                name: "AuditApplications");

            migrationBuilder.DropTable(
                name: "AuditLevels");

            migrationBuilder.DropTable(
                name: "AuditTypes");
        }
    }
}
