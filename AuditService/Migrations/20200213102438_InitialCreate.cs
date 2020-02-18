using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuditService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLevels",
                columns: table => new
                {
                    AuditLevelID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Description = table.Column<string>(nullable: false)
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
                        .Annotation("MySQL:AutoIncrement", true),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTypes", x => x.AuditTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    AuditID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    AuditTypeId = table.Column<int>(nullable: false),
                    Source = table.Column<string>(nullable: false),
                    AuditLevelId = table.Column<int>(nullable: false),
                    Details = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.AuditID);
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
                name: "AuditLevels");

            migrationBuilder.DropTable(
                name: "AuditTypes");
        }
    }
}
