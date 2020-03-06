using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EA.Audit.AuditService.Migrations
{
    public partial class TenancyIdType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "Audits",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AuditApplications",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Audits",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AuditApplications",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
