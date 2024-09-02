using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CaseNumber = table.Column<int>(type: "int", nullable: false, comment: "案件編號"),
                    AdminUserId = table.Column<int>(type: "int", nullable: true, comment: "指派人"),
                    ApplicantAccount = table.Column<string>(type: "longtext", nullable: false, comment: "申請人帳號"),
                    ApplicantName = table.Column<string>(type: "longtext", nullable: false, comment: "申請人姓名"),
                    ApplicationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "申請日期"),
                    UnitName = table.Column<string>(type: "longtext", nullable: true, comment: "單位名稱"),
                    County = table.Column<string>(type: "longtext", nullable: false, comment: "聯絡人縣市"),
                    District = table.Column<string>(type: "longtext", nullable: false, comment: "聯絡人區域"),
                    Address = table.Column<string>(type: "longtext", nullable: false, comment: "聯絡人地址"),
                    Phone = table.Column<string>(type: "longtext", nullable: false, comment: "電話"),
                    Fax = table.Column<string>(type: "longtext", nullable: true, comment: "傳真"),
                    Email = table.Column<string>(type: "longtext", nullable: false, comment: "Email"),
                    DamageTreeCounty = table.Column<string>(type: "longtext", nullable: false, comment: "受害樹木縣市"),
                    DamageTreeDistrict = table.Column<string>(type: "longtext", nullable: false, comment: "受害樹木區域"),
                    DamageTreeAddress = table.Column<string>(type: "longtext", nullable: false, comment: "受害樹木地址"),
                    ForestCompartmentLocationId = table.Column<int>(type: "int", nullable: false, comment: "林班位置"),
                    ForestSection = table.Column<string>(type: "longtext", nullable: true, comment: "林班"),
                    ForestSubsection = table.Column<string>(type: "longtext", nullable: true, comment: "小班"),
                    Latitude = table.Column<string>(type: "longtext", nullable: true, comment: "緯度"),
                    Longitude = table.Column<string>(type: "longtext", nullable: true, comment: "經度"),
                    DamagedArea = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "受損面積"),
                    DamagedCount = table.Column<int>(type: "int", nullable: true, comment: "受損數量"),
                    PlantedArea = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "種植面積"),
                    PlantedCount = table.Column<int>(type: "int", nullable: true, comment: "種植數量"),
                    TreeBasicInfoId = table.Column<int>(type: "int", nullable: false, comment: "樹木基本資料"),
                    Others = table.Column<string>(type: "longtext", nullable: true, comment: "其他"),
                    DamagedPart = table.Column<string>(type: "longtext", nullable: false, comment: "受害部位 1 = 根, 莖、4 = 枝條, 6 = 樹葉, 7 = 花果, 8 = 全株"),
                    TreeHeight = table.Column<string>(type: "longtext", nullable: false, comment: "樹木高度"),
                    TreeDiameter = table.Column<string>(type: "longtext", nullable: false, comment: "樹木直徑"),
                    LocalPlantingTime = table.Column<string>(type: "longtext", nullable: true, comment: "現地種植時間"),
                    FirstDiscoveryDate = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "首次發現受害時間"),
                    DamageDescription = table.Column<string>(type: "longtext", nullable: false, comment: "受害症狀描述"),
                    LocationType = table.Column<string>(type: "longtext", nullable: false, comment: "立地種類 1 = 公園、校園, 人行道 = 2, 花台內 = 3, 建築周邊 = 4, 林地 = 5, 苗圃 = 6, 農地 = 7 , 空地 = 8"),
                    BaseCondition = table.Column<string>(type: "longtext", nullable: false, comment: "樹基部狀況 1 = 水泥面, 2 = 柏油面, 3 = 植被泥土面 (地表有草皮或鬆潤木), 4 = 花台內, 5 = 人工鋪面 (水泥面、柏油面以外)"),
                    Photo = table.Column<string>(type: "longtext", nullable: false, comment: "圖片"),
                    CaseStatus = table.Column<int>(type: "int", nullable: false, comment: "案件狀態 1 = 暫存, 2 = 待指派, 3 = 待簽核, 4 = 已結案, 5 = 已刪除, 6 = 退回"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseRecord_ForestCompartmentLocation_ForestCompartmentLocati~",
                        column: x => x.ForestCompartmentLocationId,
                        principalTable: "ForestCompartmentLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseRecord_TreeBasicInfo_TreeBasicInfoId",
                        column: x => x.TreeBasicInfoId,
                        principalTable: "TreeBasicInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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
                        name: "FK_CaseDiagnosisResult_CaseRecord_CaseId",
                        column: x => x.CaseId,
                        principalTable: "CaseRecord",
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

            migrationBuilder.CreateIndex(
                name: "IX_CaseRecord_ForestCompartmentLocationId",
                table: "CaseRecord",
                column: "ForestCompartmentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseRecord_TreeBasicInfoId",
                table: "CaseRecord",
                column: "TreeBasicInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseDiagnosisResult");

            migrationBuilder.DropTable(
                name: "CaseRecord");
        }
    }
}
