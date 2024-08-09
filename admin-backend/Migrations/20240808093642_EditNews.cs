using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EditNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WebsiteReleases",
                table: "News",
                type: "int",
                nullable: false,
                comment: "發佈網站 1 = 林業自然保育署, 2 = 林業試驗所",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "發佈網站");

            migrationBuilder.AddColumn<int>(
                name: "AdminUserId",
                table: "News",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "發佈者");

            migrationBuilder.CreateIndex(
                name: "IX_News_AdminUserId",
                table: "News",
                column: "AdminUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AdminUser_AdminUserId",
                table: "News",
                column: "AdminUserId",
                principalTable: "AdminUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AdminUser_AdminUserId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_AdminUserId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "News");

            migrationBuilder.AlterColumn<int>(
                name: "WebsiteReleases",
                table: "News",
                type: "int",
                nullable: false,
                comment: "發佈網站",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "發佈網站 1 = 林業自然保育署, 2 = 林業試驗所");
        }
    }
}
