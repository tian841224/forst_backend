using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EditFAQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminUserId",
                table: "FAQ",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "發佈者");

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "FAQ",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "排序");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_AdminUserId",
                table: "FAQ",
                column: "AdminUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FAQ_AdminUser_AdminUserId",
                table: "FAQ",
                column: "AdminUserId",
                principalTable: "AdminUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FAQ_AdminUser_AdminUserId",
                table: "FAQ");

            migrationBuilder.DropIndex(
                name: "IX_FAQ_AdminUserId",
                table: "FAQ");

            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "FAQ");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "FAQ");
        }
    }
}
