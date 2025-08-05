using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduFlow.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompanyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Registry_registry_id",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_registry_id",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "registry_id",
                table: "Companies");

            migrationBuilder.AddColumn<long>(
                name: "company_id",
                table: "Registry",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Registry_company_id",
                table: "Registry",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Registry_Companies_company_id",
                table: "Registry",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registry_Companies_company_id",
                table: "Registry");

            migrationBuilder.DropIndex(
                name: "IX_Registry_company_id",
                table: "Registry");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Registry");

            migrationBuilder.AddColumn<long>(
                name: "registry_id",
                table: "Companies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_registry_id",
                table: "Companies",
                column: "registry_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Registry_registry_id",
                table: "Companies",
                column: "registry_id",
                principalTable: "Registry",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
