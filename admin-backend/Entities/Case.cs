using admin_backend.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace admin_backend.Entities
{
    public class Case : DefaultEntity
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        [Required]
        [Comment("案件編號")]
        public int CaseNumber { get; set; } 

        /// <summary>
        /// 指派人
        /// </summary>
        [Comment("指派人")]
        public int AdminUserId { get; set; }

        /// <summary>
        /// 申請人
        /// </summary>
        [Required]
        [Comment("申請人")]
        public int UserId { get; set; }

        /// <summary>
        /// 申請日期
        /// </summary>
        [Required]
        [Comment("申請日期")]
        public DateTime ApplicationDate { get; set; }

        /// <summary>
        /// 單位名稱
        /// </summary>
        [Comment("單位名稱")]
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 聯絡人縣市
        /// </summary>
        [Required]
        [Comment("聯絡人樹木縣市")]
        public string County { get; set; } = string.Empty;

        /// <summary>
        /// 聯絡人區域
        /// </summary>
        [Required]
        [Comment("聯絡人區域")]
        public string District { get; set; } = string.Empty;

        /// <summary>
        /// 聯絡人地址
        /// </summary>
        [Required]
        [Comment("聯絡人地址")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 電話
        /// </summary>
        [Required]
        [Comment("電話")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 傳真
        /// </summary>
        [Comment("傳真")]
        public string Fax { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [EmailAddress]
        [Comment("Email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 受害樹木縣市
        /// </summary>
        [Required]
        [Comment("受害樹木縣市")]
        public string DamageTreeCounty { get; set; } = string.Empty;

        /// <summary>
        /// 受害樹木區域
        /// </summary>
        [Required]
        [Comment("受害樹木區域")]
        public string DamageTreeDistrict { get; set; } = string.Empty;

        /// <summary>
        /// 受害樹木地址
        /// </summary>
        [Required]
        [Comment("受害樹木地址")]
        public string DamageTreeAddress { get; set; } = string.Empty;

        /// <summary>
        /// 林班位置
        /// </summary>
        [Required]
        [Comment("林班位置")]
        [StringLength(100)]
        public string ForestSectionLocation { get; set; } = string.Empty;

        /// <summary>
        /// 所屬管理處
        /// </summary>
        [Required]
        [Comment("所屬管理處")]
        [StringLength(100)]
        public string AffiliatedUnit { get; set; } = string.Empty;

        /// <summary>
        /// 林班
        /// </summary>
        [Comment("林班")]
        public string ForestSection { get; set; } = string.Empty;

        /// <summary>
        /// 小班
        /// </summary>
        [Comment("小班")]
        public string ForestSubsection { get; set; } = string.Empty;

        /// <summary>
        /// 緯度/TGOS
        /// </summary>
        [Comment("緯度/TGOS")]
        public string LatitudeTgos { get; set; }

        /// <summary>
        /// 緯度/Google
        /// </summary>
        [Comment("緯度/Google")]
        public string LatitudeGoogle { get; set; }

        /// <summary>
        /// 經度/TGOS
        /// </summary>
        [Comment("經度/TGOS")]
        public string LongitudeTgos { get; set; }

        /// <summary>
        /// 經度/Google
        /// </summary>
        [Comment("經度/Google")]
        public string LongitudeGoogle { get; set; }

        /// <summary>
        /// 受損面積
        /// </summary>
        [Comment("受損面積")]
        public decimal DamagedArea { get; set; }

        /// <summary>
        /// 受損數量
        /// </summary>
        [Comment("受損數量")]
        public int DamagedCount { get; set; }

        /// <summary>
        /// 種植面積
        /// </summary>
        [Comment("種植面積")]
        public decimal PlantedArea { get; set; }

        /// <summary>
        /// 種植數量
        /// </summary>
        [Comment("種植數量")]
        public int PlantedCount { get; set; }

        /// <summary>
        /// 樹木基本資料
        /// </summary>
        [Required]
        [Comment("樹木基本資料")]
        public int TreeBasicInfoId { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        [Comment("其他")]
        public string Others { get; set; }

        /// <summary>
        /// 受害部位
        /// </summary>
        [Required]
        [Comment("受害部位 1 = 根, 莖、4 = 枝條, 6 = 樹葉, 7 = 花果, 8 = 全株")]
        public List<TreePartEnum> DamagedPart { get; set; } = new();


        /// <summary>
        /// 樹木高度
        /// </summary>
        [Required]
        [Comment("樹木高度")]
        public decimal TreeHeight { get; set; }

        /// <summary>
        /// 樹木直徑
        /// </summary>
        [Required]
        [Comment("樹木直徑")]
        public decimal TreeDiameter { get; set; }

        /// <summary>
        /// 現地種植時間
        /// </summary>
        [Comment("現地種植時間")]
        public int LocalPlantingTime { get; set; }

        /// <summary>
        /// 首次發現受害時間
        /// </summary>
        [Comment("首次發現受害時間")]
        public DateTime FirstDiscoveryDate { get; set; }

        /// <summary>
        /// 受害症狀描述
        /// </summary>
        [Required]
        [Comment("受害症狀描述")]
        public string DamageDescription { get; set; } = string.Empty;

        /// <summary>
        /// 立地種類
        /// </summary>
        [Required]
        [Comment("立地種類 1 = 公園、校園, 人行道 = 2, 花台內 = 3, 建築周邊 = 4, 林地 = 5, 苗圃 = 6, 農地 = 7 , 空地 = 8")]
        public List<LocationTypeEnum> LocationType { get; set; } = new();

        /// <summary>
        /// 樹基部狀況
        /// </summary>
        [Required]
        [Comment("樹基部狀況 1 = 水泥面 = 2, 柏油面 = 3, 植被泥土面 (地表有草皮或鬆潤木) = 4, 花台內 = 5, 人工鋪面 (水泥面、柏油面以外) = 6")]
        public List<TreeBaseConditionEnum> BaseCondition { get; set; } = new();

        /// <summary>
        /// 圖片
        /// </summary>
        [Comment("圖片")]
        public string Photo { get; set; } = string.Empty;

        /// <summary>
        /// 案件狀態
        /// </summary>
        [Comment("案件狀態")]
        public CaseStatusEnum CaseStatus { get; set; }

        /// <summary>
        /// 送件方式
        /// </summary>
        [Comment("送件方式")]
        public List<SubmissionMethodEnum>? SubmissionMethod { get; set; } = new();

        /// <summary>
        /// 送件方式
        /// </summary>
        [Comment("診斷方式")]
        public List<DiagnosisMethodEnum>? DiagnosisMethod { get; set; } = new();

        ///案件回覆////

        /// <summary>
        /// 危害狀況詳細描述
        /// </summary>
        [Comment("危害狀況詳細描述")]
        public string? HarmPatternDescription { get; set; }

        /// <summary>
        /// 危害種類
        /// </summary>
        [Comment("危害種類")]
        public string? DamageTypeName { get; set; }

        /// <summary>
        /// 危害類別
        /// </summary>
        [Comment("危害類別")]
        public string? DamageClassName { get; set; }

        /// <summary>
        /// 危害病蟲名稱
        /// </summary>
        [Comment("危害病蟲名稱")]
        public string? CommonDamageName { get; set; }

        /// <summary>
        /// 防治建議
        /// </summary>
        [Comment("防治建議")]
        public string? PreventionSuggestion { get; set; }

        /// <summary>
        /// 防治建議圖片
        /// </summary>
        [Comment("防治建議圖片")]
        public string? PreventionSuggestionPhoto { get; set; }

        /// <summary>
        /// 危害病蟲名稱(舊)
        /// </summary>
        [Comment("危害病蟲名稱(舊)")]
        public string? OldCommonDamageName { get; set; }

        /// <summary>
        /// 學名
        /// </summary>
        [Comment("學名")]
        public string? ScientificName {  get; set; }

        /// <summary>
        /// 呈報建議
        /// </summary>
        [Comment("呈報建議")]
        public string? ReportingSuggestion { get; set; }

        /// <summary>
        /// 呈報建議圖片
        /// </summary>
        [Comment("呈報建議圖片")]
        public string? ReportingSuggestionPhoto { get; set; }

        /// <summary>
        /// 退回原因
        /// </summary>
        [Comment("退回原因")]
        public string? ReturnReason { get; set; }  


        [ForeignKey("AdminUserId")]
        public virtual AdminUser AdminUser { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("TreeBasicInfoId")]
        public virtual TreeBasicInfo TreeBasicInfo { get; set; } = null!;
    }
}
