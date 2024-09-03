using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EditDefaultEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "TreeBasicInfo",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "TreeBasicInfo",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "ForestDiseasePublications",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "ForestDiseasePublications",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "ForestCompartmentLocation",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "ForestCompartmentLocation",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "FAQ",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "FAQ",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "DamageType",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "DamageType",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "CommonDamage",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "CommonDamage",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "TreeBasicInfo",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "TreeBasicInfo",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "ForestDiseasePublications",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "ForestDiseasePublications",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "ForestCompartmentLocation",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "ForestCompartmentLocation",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "FAQ",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "FAQ",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "DamageType",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "DamageType",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "CommonDamage",
                type: "datetime(6)",
                nullable: false,
                comment: "更新時間",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "更新時間")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "CommonDamage",
                type: "datetime(6)",
                nullable: false,
                comment: "建立日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "建立日期")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);
        }
    }
}
