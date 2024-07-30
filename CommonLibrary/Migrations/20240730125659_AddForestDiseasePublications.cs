using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddForestDiseasePublications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForestDiseasePublications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "出版品類型 林業叢刊 = 1, 相關摺頁 = 2"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "出版品名稱"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "出版品日期"),
                    Link = table.Column<string>(type: "longtext", nullable: false, comment: "出版品連結"),
                    Unit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "出版單位"),
                    File = table.Column<string>(type: "longtext", nullable: false, comment: "出版品檔案"),
                    Author = table.Column<string>(type: "longtext", nullable: false, comment: "出版品作者"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "狀態 0 = 關閉, 1 = 開啟"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForestDiseasePublications", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForestDiseasePublications");
        }
    }
}
