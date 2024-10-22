using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddCommonDamage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommonDamage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DamageTypeId = table.Column<int>(type: "int", nullable: false, comment: "危害類型ID"),
                    DamageClassId = table.Column<int>(type: "int", nullable: false, comment: "危害種類ID"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "病蟲危害名稱"),
                    DamagePart = table.Column<string>(type: "longtext", nullable: false, comment: "危害部位"),
                    DamageFeatures = table.Column<string>(type: "longtext", nullable: false, comment: "危害特徵"),
                    Suggestions = table.Column<string>(type: "longtext", nullable: false, comment: "危害類型ID"),
                    Photo = table.Column<string>(type: "longtext", nullable: false, comment: "危害類型ID"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "狀態 0 = 關閉, 1 = 開啟"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonDamage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonDamage_DamageClass_DamageClassId",
                        column: x => x.DamageClassId,
                        principalTable: "DamageClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommonDamage_DamageType_DamageTypeId",
                        column: x => x.DamageTypeId,
                        principalTable: "DamageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CommonDamage_DamageClassId",
                table: "CommonDamage",
                column: "DamageClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonDamage_DamageTypeId",
                table: "CommonDamage",
                column: "DamageTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommonDamage");
        }
    }
}
