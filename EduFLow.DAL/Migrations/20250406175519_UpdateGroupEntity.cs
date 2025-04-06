using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduFlow.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGroupEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Courses_CourseId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Groups",
                newName: "course_id");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CourseId",
                table: "Groups",
                newName: "IX_Groups_course_id");

            migrationBuilder.AlterColumn<long>(
                name: "course_id",
                table: "Groups",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Courses_course_id",
                table: "Groups",
                column: "course_id",
                principalTable: "Courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Courses_course_id",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "course_id",
                table: "Groups",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_course_id",
                table: "Groups",
                newName: "IX_Groups_CourseId");

            migrationBuilder.AlterColumn<long>(
                name: "CourseId",
                table: "Groups",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Courses_CourseId",
                table: "Groups",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "id");
        }
    }
}
