using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EditCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseDiagnosisResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CaseId = table.Column<int>(type: "int", nullable: false, comment: "案件編號"),
                    SubmissionMethod = table.Column<string>(type: "longtext", nullable: false, comment: "送件方式"),
                    DiagnosisMethod = table.Column<string>(type: "longtext", nullable: false, comment: "診斷方式"),
                    HarmPatternDescription = table.Column<string>(type: "longtext", nullable: false, comment: "危害狀況詳細描述"),
                    CommonDamageId = table.Column<int>(type: "int", nullable: false, comment: "常見病蟲害"),
                    PreventionSuggestion = table.Column<string>(type: "longtext", nullable: false, comment: "防治建議"),
                    OldCommonDamageName = table.Column<string>(type: "longtext", nullable: false, comment: "危害病蟲名稱(舊)"),
                    ScientificName = table.Column<string>(type: "longtext", nullable: false, comment: "學名"),
                    ReportingSuggestion = table.Column<string>(type: "longtext", nullable: false, comment: "呈報建議"),
                    ReturnReason = table.Column<string>(type: "longtext", nullable: false, comment: "退回原因"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseDiagnosisResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseDiagnosisResult_Case_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Case",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseDiagnosisResult_CommonDamage_CommonDamageId",
                        column: x => x.CommonDamageId,
                        principalTable: "CommonDamage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CaseDiagnosisResult_CaseId",
                table: "CaseDiagnosisResult",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseDiagnosisResult_CommonDamageId",
                table: "CaseDiagnosisResult",
                column: "CommonDamageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseDiagnosisResult");
        }
    }
}
