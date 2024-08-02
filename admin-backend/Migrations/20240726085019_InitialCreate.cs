using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CommonLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "名稱"),
                    Website = table.Column<int>(type: "int", nullable: false, comment: "站台 1 = 林業自然保育署, 2 = 林業試驗所"),
                    Position = table.Column<int>(type: "int", nullable: false, comment: "廣告位置 1 = 橫幅, 2 = 首頁"),
                    PhotoPc = table.Column<string>(type: "longtext", nullable: true, comment: "PC圖片"),
                    PhotoMobile = table.Column<string>(type: "longtext", nullable: true, comment: "手機圖片"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdSetting", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DamageType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "危害類型"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "狀態 0 = 關閉, 1 = 開啟"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageType", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Documentation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "使用條款類型 1 = 同意書, 2 = 使用說明"),
                    Content = table.Column<string>(type: "longtext", nullable: false, comment: "內容"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentation", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EpidemicSummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false, comment: "標題"),
                    Content = table.Column<string>(type: "longtext", nullable: false, comment: "內容"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpidemicSummary", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FAQ",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Question = table.Column<string>(type: "longtext", nullable: false, comment: "問題"),
                    Answer = table.Column<string>(type: "longtext", nullable: false, comment: "答案"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "狀態 0 = 關閉, 1 = 開啟"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQ", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Level = table.Column<string>(type: "longtext", nullable: false),
                    Message = table.Column<string>(type: "longtext", nullable: false),
                    Logger = table.Column<string>(type: "longtext", nullable: false),
                    Exception = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MailConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Host = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "主機"),
                    Port = table.Column<byte>(type: "tinyint unsigned", nullable: false, comment: "Port"),
                    Encrypted = table.Column<byte>(type: "tinyint unsigned", nullable: false, comment: "加密方式 1 = SSL, 2 = TSL"),
                    Account = table.Column<string>(type: "longtext", nullable: false, comment: "寄信帳號"),
                    Password = table.Column<string>(type: "longtext", nullable: false, comment: "寄信密碼"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "顯示名稱"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailConfig", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false, comment: "標題"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "公告類型 一般公告 = 1, 重要公告 = 2, 活動公告 = 3, 跑馬燈 = 4"),
                    Content = table.Column<string>(type: "longtext", nullable: false, comment: "發佈內容"),
                    Pinned = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "置頂"),
                    WebsiteReleases = table.Column<int>(type: "int", nullable: false, comment: "發佈網站"),
                    Schedule = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否開啟排程"),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "排程開始時間"),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "排程結束時間"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "發佈狀態 0 = 未發佈, 1 = 發佈"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "權限名稱"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "檢視 = 1, 新增 = 2, 指派 = 3, 編輯 = 4, 刪除 = 5"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "角色名稱"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Account = table.Column<string>(type: "longtext", nullable: false, comment: "帳號"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "使用者名稱"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "狀態 0 = 關閉, 1 = 開啟, 2 = 停用"),
                    LoginTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最後登入時間"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DamageClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "危害種類"),
                    DamageTypeId = table.Column<int>(type: "int", nullable: false, comment: "危害類型ID"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "狀態 0 = 關閉, 1 = 開啟"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DamageClass_DamageType_DamageTypeId",
                        column: x => x.DamageTypeId,
                        principalTable: "DamageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdminUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "姓名"),
                    Account = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "帳號"),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "密碼"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "信箱"),
                    Photo = table.Column<string>(type: "longtext", nullable: false, comment: "照片"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "角色"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "狀態 0 = 關閉, 1 = 開啟"),
                    LoginTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "登入時間"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminUser_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommonPest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DamageTypeId = table.Column<int>(type: "int", nullable: false, comment: "危害類型"),
                    DamageClassId = table.Column<int>(type: "int", nullable: false, comment: "危害種類"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "病蟲危害名稱"),
                    DamagePart = table.Column<string>(type: "longtext", nullable: false, comment: "危害部位 2 = 侵害土壤部, 3 = 樹幹, 5 = 樹枝, 6 = 樹葉, 7 = 花, 9 = 全面異常症狀病害"),
                    DamageCharacteristics = table.Column<string>(type: "longtext", nullable: false, comment: "危害特徵"),
                    ControlRecommendations = table.Column<string>(type: "longtext", nullable: false, comment: "防治建議"),
                    Photo = table.Column<string>(type: "longtext", nullable: false, comment: "病蟲封面圖片"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "狀態 0 = 關閉, 1 = 開啟"),
                    UnpublishDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "下架日期"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonPest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonPest_DamageClass_DamageClassId",
                        column: x => x.DamageClassId,
                        principalTable: "DamageClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommonPest_DamageType_DamageTypeId",
                        column: x => x.DamageTypeId,
                        principalTable: "DamageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OperationLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AdminUserId = table.Column<int>(type: "int", nullable: false, comment: "使用者ID"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "異動類型 新增 = 1, 指派 = 2, 編輯 = 3, 刪除 = 4"),
                    Content = table.Column<string>(type: "longtext", nullable: false, comment: "異動內容"),
                    Ip = table.Column<string>(type: "longtext", nullable: false, comment: "IP"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "建立日期")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新時間")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationLog_AdminUser_AdminUserId",
                        column: x => x.AdminUserId,
                        principalTable: "AdminUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AdminUser_RoleId",
                table: "AdminUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonPest_DamageClassId",
                table: "CommonPest",
                column: "DamageClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonPest_DamageTypeId",
                table: "CommonPest",
                column: "DamageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DamageClass_DamageTypeId",
                table: "DamageClass",
                column: "DamageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLog_AdminUserId",
                table: "OperationLog",
                column: "AdminUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdSetting");

            migrationBuilder.DropTable(
                name: "CommonPest");

            migrationBuilder.DropTable(
                name: "Documentation");

            migrationBuilder.DropTable(
                name: "EpidemicSummary");

            migrationBuilder.DropTable(
                name: "FAQ");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "MailConfig");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "OperationLog");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "DamageClass");

            migrationBuilder.DropTable(
                name: "AdminUser");

            migrationBuilder.DropTable(
                name: "DamageType");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
