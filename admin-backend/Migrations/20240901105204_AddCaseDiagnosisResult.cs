using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseDiagnosisResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BaseCondition",
                table: "CaseRecord",
                type: "longtext",
                nullable: false,
                comment: "樹基部狀況 1 = 水泥面, 2 = 柏油面, 3 = 植被泥土面 (地表有草皮或鬆潤木), 4 = 花台內, 5 = 人工鋪面 (水泥面、柏油面以外)",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "樹基部狀況 1 = 水泥面 = 2, 柏油面 = 3, 植被泥土面 (地表有草皮或鬆潤木) = 4, 花台內 = 5, 人工鋪面 (水泥面、柏油面以外) = 6");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseDiagnosisResult_CaseRecord_CaseId",
                table: "CaseDiagnosisResult",
                column: "CaseId",
                principalTable: "CaseRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseDiagnosisResult_CommonDamage_CommonDamageId",
                table: "CaseDiagnosisResult",
                column: "CommonDamageId",
                principalTable: "CommonDamage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseDiagnosisResult_CaseRecord_CaseId",
                table: "CaseDiagnosisResult");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseDiagnosisResult_CommonDamage_CommonDamageId",
                table: "CaseDiagnosisResult");

            migrationBuilder.AlterColumn<string>(
                name: "BaseCondition",
                table: "CaseRecord",
                type: "longtext",
                nullable: false,
                comment: "樹基部狀況 1 = 水泥面 = 2, 柏油面 = 3, 植被泥土面 (地表有草皮或鬆潤木) = 4, 花台內 = 5, 人工鋪面 (水泥面、柏油面以外) = 6",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "樹基部狀況 1 = 水泥面, 2 = 柏油面, 3 = 植被泥土面 (地表有草皮或鬆潤木), 4 = 花台內, 5 = 人工鋪面 (水泥面、柏油面以外)");
        }
    }
}
