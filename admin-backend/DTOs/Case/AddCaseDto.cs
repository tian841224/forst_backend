using admin_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.Case
{
    public class AddCaseDto
    {
        /// <summary>
        /// 申請人ID
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 申請日期
        /// </summary>
        public DateTime ApplicationDate { get; set; }

        /// <summary>
        /// 單位名稱
        /// </summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 縣市
        /// </summary>
        [Required]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// 地址
        /// </summary>
        [Required]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 電話
        /// </summary>
        [Required]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 傳真
        /// </summary>
        public string Fax { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 受害樹木地址
        /// </summary>
        [Required]
        public string CaseAddress { get; set; } = string.Empty;

        /// <summary>
        /// 林班位置
        /// </summary>
        public string ForestSectionLocation { get; set; } = string.Empty;

        /// <summary>
        /// 所屬管理處
        /// </summary>
        public string AffiliatedUnit { get; set; } = string.Empty;

        /// <summary>
        /// 林班
        /// </summary>
        public string ForestSection { get; set; } = string.Empty;

        /// <summary>
        /// 小班
        /// </summary>
        public string ForestSubsection { get; set; } = string.Empty;

        /// <summary>
        /// 緯度/TGOS
        /// </summary>
        public string LatitudeTgos { get; set; }

        /// <summary>
        /// 緯度/Google
        /// </summary>
        public string LatitudeGoogle { get; set; }

        /// <summary>
        /// 經度/TGOS
        /// </summary>
        public string LongitudeTgos { get; set; }

        /// <summary>
        /// 經度/Google
        /// </summary>
        public string LongitudeGoogle { get; set; }

        /// <summary>
        /// 受損面積
        /// </summary>
        public decimal DamagedArea { get; set; }

        /// <summary>
        /// 受損數量
        /// </summary>
        public int DamagedCount { get; set; }

        /// <summary>
        /// 種植面積
        /// </summary>
        public decimal PlantedArea { get; set; }

        /// <summary>
        /// 種植數量
        /// </summary>
        public int PlantedCount { get; set; }

        /// <summary>
        /// 樹木基本資料
        /// </summary>
        public int TreeBasicInfoId { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        public string Others { get; set; } = string.Empty;

        /// <summary>
        /// 危害部位 2 = 侵害土壤部, 3 = 樹幹, 5 = 樹枝, 6 = 樹葉, 7 = 花, 9 = 全面異常症狀病害
        /// </summary>
        [Required]
        public List<TreePartEnum> DamagedPart { get; set; } = new();

        /// <summary>
        /// 樹木高度
        /// </summary>
        [Required]
        public decimal TreeHeight { get; set; }

        /// <summary>
        /// 樹木直徑
        /// </summary>
        [Required]
        public decimal TreeDiameter { get; set; }

        /// <summary>
        /// 現地種植時間
        /// </summary>
        public int LocalPlantingTime { get; set; }

        /// <summary>
        /// 首次發現受害時間
        /// </summary>
        public DateTime FirstDiscoveryDate { get; set; }

        /// <summary>
        /// 受害症狀描述
        /// </summary>
        [Required]
        public string DamageDescription { get; set; } = string.Empty;

        /// <summary>
        /// 立地種類 1 = 公園、校園, 人行道 = 2, 花台內 = 3, 建築周邊 = 4, 林地 = 5, 苗圃 = 6, 農地 = 7 , 空地 = 8
        /// </summary>
        public List<LocationTypeEnum> LocationType { get; set; } = new();

        /// <summary>
        /// 樹基部狀況 1 = 水泥面 = 2, 柏油面 = 3, 植被泥土面 (地表有草皮或鬆潤木) = 4, 花台內 = 5, 人工鋪面 (水泥面、柏油面以外) = 6
        /// </summary>.
        public List<TreeBaseConditionEnum> BaseCondition { get; set; } = new();

        /// <summary>
        /// 圖片
        /// </summary>
        public List<IFormFile> Photo { get; set; } 

        ///// <summary>
        ///// 案件狀態
        ///// </summary>
        //public CaseStatusEnum CaseStatus { get; set; }
    }
}
