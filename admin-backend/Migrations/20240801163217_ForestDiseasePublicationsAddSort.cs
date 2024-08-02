using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ForestDiseasePublicationsAddSort : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "ForestDiseasePublications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "排序");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sort",
                table: "ForestDiseasePublications");
        }
    }
}
