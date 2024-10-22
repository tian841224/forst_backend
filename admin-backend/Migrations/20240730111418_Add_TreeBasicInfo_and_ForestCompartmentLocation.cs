using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class Add_TreeBasicInfo_and_ForestCompartmentLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForestCompartmentLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Postion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "位置"),
                    AffiliatedUnit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "所屬管理處"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForestCompartmentLocation", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TreeBasicInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ScientificName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "學名"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "名稱"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeBasicInfo", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForestCompartmentLocation");

            migrationBuilder.DropTable(
                name: "TreeBasicInfo");
        }
    }
}
