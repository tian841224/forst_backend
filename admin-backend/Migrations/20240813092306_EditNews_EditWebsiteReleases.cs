using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EditNews_EditWebsiteReleases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WebsiteReleases",
                table: "News",
                type: "longtext",
                nullable: false,
                comment: "發佈網站 1 = 林業自然保育署, 2 = 林業試驗所",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "發佈網站 1 = 林業自然保育署, 2 = 林業試驗所");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WebsiteReleases",
                table: "News",
                type: "int",
                nullable: false,
                comment: "發佈網站 1 = 林業自然保育署, 2 = 林業試驗所",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "發佈網站 1 = 林業自然保育署, 2 = 林業試驗所");
        }
    }
}
