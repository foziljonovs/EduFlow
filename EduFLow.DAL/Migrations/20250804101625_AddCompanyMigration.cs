using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EduFlow.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "company_id",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "company_id",
                table: "Teachers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "company_id",
                table: "Students",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "company_id",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "company_id",
                table: "Messages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "company_id",
                table: "Groups",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "company_id",
                table: "Courses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "company_id",
                table: "Categories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Logos",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    content_type = table.Column<string>(type: "text", nullable: true),
                    file_path = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    about = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    logo_id = table.Column<long>(type: "bigint", nullable: false),
                    registry_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.id);
                    table.ForeignKey(
                        name: "FK_Companies_Logos_logo_id",
                        column: x => x.logo_id,
                        principalTable: "Logos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Companies_Registry_registry_id",
                        column: x => x.registry_id,
                        principalTable: "Registry",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanySubscriptions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    is_paid = table.Column<bool>(type: "boolean", nullable: false),
                    SubcriptionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySubscriptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_CompanySubscriptions_Companies_company_id",
                        column: x => x.company_id,
                        principalTable: "Companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_company_id",
                table: "Users",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_company_id",
                table: "Teachers",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Students_company_id",
                table: "Students",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_company_id",
                table: "Payments",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_company_id",
                table: "Messages",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_company_id",
                table: "Groups",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_company_id",
                table: "Courses",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_company_id",
                table: "Categories",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_logo_id",
                table: "Companies",
                column: "logo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_registry_id",
                table: "Companies",
                column: "registry_id");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySubscriptions_company_id",
                table: "CompanySubscriptions",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Companies_company_id",
                table: "Categories",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Companies_company_id",
                table: "Courses",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Companies_company_id",
                table: "Groups",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Companies_company_id",
                table: "Messages",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Companies_company_id",
                table: "Payments",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Companies_company_id",
                table: "Students",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Companies_company_id",
                table: "Teachers",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_company_id",
                table: "Users",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Companies_company_id",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Companies_company_id",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Companies_company_id",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Companies_company_id",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Companies_company_id",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Companies_company_id",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Companies_company_id",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_company_id",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CompanySubscriptions");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Logos");

            migrationBuilder.DropIndex(
                name: "IX_Users_company_id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_company_id",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_company_id",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Payments_company_id",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Messages_company_id",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Groups_company_id",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Courses_company_id",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Categories_company_id",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Categories");
        }
    }
}
