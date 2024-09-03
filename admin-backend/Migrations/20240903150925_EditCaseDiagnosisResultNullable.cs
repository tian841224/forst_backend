using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EditCaseDiagnosisResultNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseDiagnosisResult_CommonDamage_CommonDamageId",
                table: "CaseDiagnosisResult");

            migrationBuilder.AlterColumn<string>(
                name: "SubmissionMethod",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: true,
                comment: "送件方式",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "送件方式");

            migrationBuilder.AlterColumn<string>(
                name: "ScientificName",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: true,
                comment: "學名",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "學名");

            migrationBuilder.AlterColumn<string>(
                name: "ReturnReason",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: true,
                comment: "退回原因",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "退回原因");

            migrationBuilder.AlterColumn<string>(
                name: "ReportingSuggestion",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: true,
                comment: "呈報建議",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "呈報建議");

            migrationBuilder.AlterColumn<string>(
                name: "PreventionSuggestion",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: true,
                comment: "防治建議",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "防治建議");

            migrationBuilder.AlterColumn<string>(
                name: "OldCommonDamageName",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: true,
                comment: "危害病蟲名稱(舊)",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "危害病蟲名稱(舊)");

            migrationBuilder.AlterColumn<string>(
                name: "HarmPatternDescription",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: true,
                comment: "危害狀況詳細描述",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "危害狀況詳細描述");

            migrationBuilder.AlterColumn<string>(
                name: "DiagnosisMethod",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: true,
                comment: "診斷方式",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "診斷方式");

            migrationBuilder.AlterColumn<int>(
                name: "CommonDamageId",
                table: "CaseDiagnosisResult",
                type: "int",
                nullable: true,
                comment: "常見病蟲害",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "常見病蟲害");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseDiagnosisResult_CommonDamage_CommonDamageId",
                table: "CaseDiagnosisResult",
                column: "CommonDamageId",
                principalTable: "CommonDamage",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseDiagnosisResult_CommonDamage_CommonDamageId",
                table: "CaseDiagnosisResult");

            migrationBuilder.AlterColumn<string>(
                name: "SubmissionMethod",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                comment: "送件方式",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "送件方式");

            migrationBuilder.AlterColumn<string>(
                name: "ScientificName",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                comment: "學名",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "學名");

            migrationBuilder.AlterColumn<string>(
                name: "ReturnReason",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                comment: "退回原因",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "退回原因");

            migrationBuilder.AlterColumn<string>(
                name: "ReportingSuggestion",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                comment: "呈報建議",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "呈報建議");

            migrationBuilder.AlterColumn<string>(
                name: "PreventionSuggestion",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                comment: "防治建議",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "防治建議");

            migrationBuilder.AlterColumn<string>(
                name: "OldCommonDamageName",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                comment: "危害病蟲名稱(舊)",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "危害病蟲名稱(舊)");

            migrationBuilder.AlterColumn<string>(
                name: "HarmPatternDescription",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                comment: "危害狀況詳細描述",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "危害狀況詳細描述");

            migrationBuilder.AlterColumn<string>(
                name: "DiagnosisMethod",
                table: "CaseDiagnosisResult",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                comment: "診斷方式",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "診斷方式");

            migrationBuilder.AlterColumn<int>(
                name: "CommonDamageId",
                table: "CaseDiagnosisResult",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "常見病蟲害",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "常見病蟲害");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseDiagnosisResult_CommonDamage_CommonDamageId",
                table: "CaseDiagnosisResult",
                column: "CommonDamageId",
                principalTable: "CommonDamage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
