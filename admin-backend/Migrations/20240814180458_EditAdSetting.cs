using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EditAdSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "AdSetting");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "AdSetting",
                type: "longtext",
                nullable: false,
                comment: "站台 1 = 林業自然保育署, 2 = 林業試驗所",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "站台 1 = 林業自然保育署, 2 = 林業試驗所");

            migrationBuilder.AddColumn<int>(
                name: "AdminUserId",
                table: "AdSetting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "發佈者");

            migrationBuilder.CreateIndex(
                name: "IX_AdSetting_AdminUserId",
                table: "AdSetting",
                column: "AdminUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdSetting_AdminUser_AdminUserId",
                table: "AdSetting",
                column: "AdminUserId",
                principalTable: "AdminUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdSetting_AdminUser_AdminUserId",
                table: "AdSetting");

            migrationBuilder.DropIndex(
                name: "IX_AdSetting_AdminUserId",
                table: "AdSetting");

            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "AdSetting");

            migrationBuilder.AlterColumn<int>(
                name: "Website",
                table: "AdSetting",
                type: "int",
                nullable: false,
                comment: "站台 1 = 林業自然保育署, 2 = 林業試驗所",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "站台 1 = 林業自然保育署, 2 = 林業試驗所");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "AdSetting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "廣告位置 1 = 橫幅, 2 = 首頁");
        }
    }
}
