using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EditForestCompartmentLocation_RenamePosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "ForestCompartmentLocation",
                nullable: true);

            migrationBuilder.Sql("UPDATE ForestCompartmentLocation SET Position = Postion");

            migrationBuilder.DropColumn(
                name: "Postion",
                table: "ForestCompartmentLocation");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Postion",
                table: "ForestCompartmentLocation",
                nullable: true);

            migrationBuilder.Sql("UPDATE ForestCompartmentLocation SET Postion = Position");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "ForestCompartmentLocation");
        }
    }
}
