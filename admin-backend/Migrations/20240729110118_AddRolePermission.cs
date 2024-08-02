using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddRolePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Encrypted",
                table: "MailConfig",
                type: "int",
                nullable: false,
                comment: "加密方式 1 = SSL, 2 = TSL",
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned",
                oldComment: "加密方式 1 = SSL, 2 = TSL");

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "選單名稱"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "角色管理ID"),
                    View = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "檢視"),
                    Add = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "新增"),
                    Sign = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "指派"),
                    Edit = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "編輯"),
                    Delete = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "刪除"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.AlterColumn<byte>(
                name: "Encrypted",
                table: "MailConfig",
                type: "tinyint unsigned",
                nullable: false,
                comment: "加密方式 1 = SSL, 2 = TSL",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "加密方式 1 = SSL, 2 = TSL");
        }
    }
}
