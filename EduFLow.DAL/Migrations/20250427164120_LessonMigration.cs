using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EduFlow.DAL.Migrations
{
    /// <inheritdoc />
    public partial class LessonMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Groups_group_id",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "Attendances",
                newName: "lesson_id");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_group_id",
                table: "Attendances",
                newName: "IX_Attendances_lesson_id");

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    lesson_number = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.id);
                    table.ForeignKey(
                        name: "FK_Lessons_Groups_group_id",
                        column: x => x.group_id,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_group_id",
                table: "Lessons",
                column: "group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Lessons_lesson_id",
                table: "Attendances",
                column: "lesson_id",
                principalTable: "Lessons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Lessons_lesson_id",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.RenameColumn(
                name: "lesson_id",
                table: "Attendances",
                newName: "group_id");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_lesson_id",
                table: "Attendances",
                newName: "IX_Attendances_group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Groups_group_id",
                table: "Attendances",
                column: "group_id",
                principalTable: "Groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
