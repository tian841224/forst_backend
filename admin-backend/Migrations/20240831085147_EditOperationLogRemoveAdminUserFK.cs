using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EditOperationLogRemoveAdminUserFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationLog_AdminUser_AdminUserId",
                table: "OperationLog");

            migrationBuilder.DropIndex(
                name: "IX_OperationLog_AdminUserId",
                table: "OperationLog");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "OperationLog",
                type: "int",
                nullable: true,
                comment: "使用者ID",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "使用者ID");

            migrationBuilder.AlterColumn<int>(
                name: "AdminUserId",
                table: "OperationLog",
                type: "int",
                nullable: true,
                comment: "後台使用者ID",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "後台使用者ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "OperationLog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "使用者ID",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "使用者ID");

            migrationBuilder.AlterColumn<int>(
                name: "AdminUserId",
                table: "OperationLog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "後台使用者ID",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "後台使用者ID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLog_AdminUserId",
                table: "OperationLog",
                column: "AdminUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationLog_AdminUser_AdminUserId",
                table: "OperationLog",
                column: "AdminUserId",
                principalTable: "AdminUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
