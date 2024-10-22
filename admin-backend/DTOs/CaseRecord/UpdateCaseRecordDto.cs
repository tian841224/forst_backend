using admin_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.Case
{
    public class UpdateCaseRecordDto
    {
        /// <summary>
        /// 指派人
        /// </summary>
        public int? AdminUserId { get; set; }

        ///// <summary>
        ///// 申請人
        ///// </summary>
        //[Required]
        //public int UserId { get; set; }

        /// <summary>
        /// 申請日期
        /// </summary>
        public string? ApplicationDate { get; set; }

        /// <summary>
        /// 單位名稱
        /// </summary>
        public string? UnitName { get; set; }

        /// <summary>
        /// 聯絡人縣市
        /// </summary>
        public string? County { get; set; }

        /// <summary>
        /// 聯絡人區域
        /// </summary>
        public string? District { get; set; }

        /// <summary>
        /// 聯絡人地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// 傳真
        /// </summary>
        public string? Fax { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// 受害樹木縣市
        /// </summary>
        public string? DamageTreeCounty { get; set; }

        /// <summary>
        /// 受害樹木區域
        /// </summary>
        public string? DamageTreeDistrict { get; set; }

        /// <summary>
        /// 受害樹木地址
        /// </summary>
        public string? DamageTreeAddress { get; set; }

        ///// <summary>
        ///// 林班位置
        ///// </summary>
        //public int? ForestCompartmentLocationId { get; set; }

        /// <summary>
        /// 林班位置
        /// </summary>
        public string? ForestPosition { get; set; } = string.Empty;

        /// <summary>
        /// 所屬管理處
        /// </summary>
        public string? AffiliatedUnit { get; set; } = string.Empty;

        /// <summary>
        /// 林班
        /// </summary>
        public string? ForestSection { get; set; }

        /// <summary>
        /// 小班
        /// </summary>
        public string? ForestSubsection { get; set; }

        /// <summary>
        /// 緯度
        /// </summary>
        public string? Latitude { get; set; }

        /// <summary>
        /// 經度
        /// </summary>
        public string? Longitude { get; set; }

        /// <summary>
        /// 受損面積
        /// </summary>
        public decimal? DamagedArea { get; set; }

        /// <summary>
        /// 受損數量
        /// </summary>
        public int? DamagedCount { get; set; }

        /// <summary>
        /// 種植面積
        /// </summary>
        public decimal? PlantedArea { get; set; }

        /// <summary>
        /// 種植數量
        /// </summary>
        public int? PlantedCount { get; set; }

        /// <summary>
        /// 樹木基本資料
        /// </summary>
        public int? TreeBasicInfoId { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        public string? Others { get; set; }

        /// <summary>
        /// 受害部位 1 = 根, 莖、4 = 枝條, 6 = 樹葉, 7 = 花果, 8 = 全株
        /// </summary>
        public List<TreePartEnum>? DamagedPart { get; set; }

        /// <summary>
        /// 樹木高度
        /// </summary>
        public string? TreeHeight { get; set; } = string.Empty;

        /// <summary>
        /// 樹木直徑
        /// </summary>
        public string? TreeDiameter { get; set; } = string.Empty;

        /// <summary>
        /// 現地種植時間
        /// </summary>
        public string? LocalPlantingTime { get; set; }

        /// <summary>
        /// 首次發現受害時間
        /// </summary>
        public string? FirstDiscoveryDate { get; set; }

        /// <summary>
        /// 受害症狀描述
        /// </summary>
        public string? DamageDescription { get; set; }

        /// <summary>
        /// 立地種類 1 = 公園、校園, 人行道 = 2, 花台內 = 3, 建築周邊 = 4, 林地 = 5, 苗圃 = 6, 農地 = 7 , 空地 = 8
        /// </summary>
        public List<LocationTypeEnum>? LocationType { get; set; }

        /// <summary>
        /// 樹基部狀況 1 = 水泥面 = 2, 柏油面 = 3, 植被泥土面 (地表有草皮或鬆潤木) = 4, 花台內 = 5, 人工鋪面 (水泥面、柏油面以外) = 6
        /// </summary>
        public List<TreeBaseConditionEnum>? BaseCondition { get; set; }

        ///// <summary>
        ///// 圖片
        ///// </summary>
        //public string? Photo { get; set; }

        /// <summary>
        /// 案件狀態 1 = 暫存, 2 = 待指派, 3 = 待簽核, 4 = 已結案, 5 = 已刪除, 6 = 退回
        /// </summary>
        public CaseStatusEnum? CaseStatus { get; set; }
    }
}
