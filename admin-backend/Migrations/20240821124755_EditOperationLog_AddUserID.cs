using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EditOperationLog_AddUserID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AdminUserId",
                table: "OperationLog",
                type: "int",
                nullable: false,
                comment: "後台使用者ID",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "使用者ID");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "OperationLog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "使用者ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OperationLog");

            migrationBuilder.AlterColumn<int>(
                name: "AdminUserId",
                table: "OperationLog",
                type: "int",
                nullable: false,
                comment: "使用者ID",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "後台使用者ID");
        }
    }
}
