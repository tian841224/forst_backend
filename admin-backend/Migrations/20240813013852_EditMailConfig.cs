using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EditMailConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Port",
                table: "MailConfig",
                type: "int",
                nullable: false,
                comment: "Port",
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned",
                oldComment: "Port");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Port",
                table: "MailConfig",
                type: "tinyint unsigned",
                nullable: false,
                comment: "Port",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Port");
        }
    }
}
