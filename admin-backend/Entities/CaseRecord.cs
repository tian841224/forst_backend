using admin_backend.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace admin_backend.Entities
{
    /// <summary>
    /// 案件
    /// </summary>
    public class CaseRecord : DefaultEntity
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
        public int? AdminUserId { get; set; }

        /// <summary>
        /// 申請人帳號
        /// </summary>
        [Required]
        [Comment("申請人帳號")]
        public string ApplicantAccount { get; set; } = string.Empty;

        /// <summary>
        /// 申請人姓名
        /// </summary>
        [Required]
        [Comment("申請人姓名")]
        public string ApplicantName { get; set; } = string.Empty;

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
        public string? UnitName { get; set; } 

        /// <summary>
        /// 聯絡人縣市
        /// </summary>
        [Required]
        [Comment("聯絡人縣市")]
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
        public string? Fax { get; set; } 

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
        public int ForestCompartmentLocationId { get; set; }

        /// <summary>
        /// 林班
        /// </summary>
        [Comment("林班")]
        public string? ForestSection { get; set; }

        /// <summary>
        /// 小班
        /// </summary>
        [Comment("小班")]
        public string? ForestSubsection { get; set; }

        /// <summary>
        /// 緯度
        /// </summary>
        [Comment("緯度")]
        public string? Latitude { get; set; }

        /// <summary>
        /// 經度
        /// </summary>
        [Comment("經度")]
        public string? Longitude { get; set; }

        /// <summary>
        /// 受損面積
        /// </summary>
        [Comment("受損面積")]
        public decimal? DamagedArea { get; set; }

        /// <summary>
        /// 受損數量
        /// </summary>
        [Comment("受損數量")]
        public int? DamagedCount { get; set; }

        /// <summary>
        /// 種植面積
        /// </summary>
        [Comment("種植面積")]
        public decimal? PlantedArea { get; set; }

        /// <summary>
        /// 種植數量
        /// </summary>
        [Comment("種植數量")]
        public int? PlantedCount { get; set; }

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
        public string? Others { get; set; }

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
        public string TreeHeight { get; set; }

        /// <summary>
        /// 樹木直徑
        /// </summary>
        [Required]
        [Comment("樹木直徑")]
        public string TreeDiameter { get; set; }

        /// <summary>
        /// 現地種植時間
        /// </summary>
        [Comment("現地種植時間")]
        public string? LocalPlantingTime { get; set; }

        /// <summary>
        /// 首次發現受害時間
        /// </summary>
        [Comment("首次發現受害時間")]
        public DateTime? FirstDiscoveryDate { get; set; }

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
        [Comment("樹基部狀況 1 = 水泥面, 2 = 柏油面, 3 = 植被泥土面 (地表有草皮或鬆潤木), 4 = 花台內, 5 = 人工鋪面 (水泥面、柏油面以外)")]
        public List<TreeBaseConditionEnum> BaseCondition { get; set; } = new();

        /// <summary>
        /// 圖片
        /// </summary>
        [Comment("圖片")]
        public string Photo { get; set; } = string.Empty;

        /// <summary>
        /// 案件狀態 1 = 暫存, 2 = 待指派, 3 = 待簽核, 4 = 已結案, 5 = 已刪除, 6 = 退回
        /// </summary>
        [Required]
        [Comment("案件狀態 1 = 暫存, 2 = 待指派, 3 = 待簽核, 4 = 已結案, 5 = 已刪除, 6 = 退回")]
        public CaseStatusEnum CaseStatus { get; set; }

        [ForeignKey("TreeBasicInfoId")]
        public virtual TreeBasicInfo TreeBasicInfo { get; set; } = null!;

        [ForeignKey("ForestCompartmentLocationId")]
        public virtual ForestCompartmentLocation ForestCompartmentLocation { get; set; } = null!;
        
    }
}
