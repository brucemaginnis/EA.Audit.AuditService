using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EA.Audit.AuditService.Migrations
{
    public partial class AddApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Audits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AuditApplicationId",
                table: "Audits",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditApplications",
                columns: table => new
                {
                    ApplicationID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditApplications", x => x.ApplicationID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audits_AuditApplicationId",
                table: "Audits",
                column: "AuditApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AuditApplications_AuditApplicationId",
                table: "Audits",
                column: "AuditApplicationId",
                principalTable: "AuditApplications",
                principalColumn: "ApplicationID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AuditApplications_AuditApplicationId",
                table: "Audits");

            migrationBuilder.DropTable(
                name: "AuditApplications");

            migrationBuilder.DropIndex(
                name: "IX_Audits_AuditApplicationId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "AuditApplicationId",
                table: "Audits");
        }
    }
}
