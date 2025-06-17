using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduFlow.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "debit",
                table: "Payments",
                newName: "amount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "amount",
                table: "Payments",
                newName: "debit");
        }
    }
}
