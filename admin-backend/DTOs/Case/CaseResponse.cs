using admin_backend.Enums;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace admin_backend.DTOs.Case
{
    public class CaseResponse : SortDefaultResponseDto
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        public int CaseNumber { get; set; }

        /// <summary>
        /// 指派人ID
        /// </summary>
        public int AdminUserId { get; set; }

        /// <summary>
        /// 指派人
        /// </summary>
        public string AdminUserName { get; set; } = string.Empty;

        /// <summary>
        /// 申請人ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 申請人
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 申請日期
        /// </summary>
        public DateTime ApplicationDate { get; set; }

        /// <summary>
        /// 單位名稱
        /// </summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 受害樹木縣市
        /// </summary>
        [Required]
        public string County { get; set; } = string.Empty;

        /// <summary>
        /// 受害樹木區域
        /// </summary>
        [Required]
        public string District { get; set; } = string.Empty;

        /// <summary>
        /// 受害樹木地址
        /// </summary>
        [Required]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 電話
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 傳真
        /// </summary>
        public string Fax { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 受害樹木地址
        /// </summary>
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
        /// 學名
        /// </summary>
        public string ScientificName { get; set; } = string.Empty;

        /// <summary>
        /// 樹木名稱名稱
        /// </summary>
        public string TreeBasicInfoName { get; set; } = string.Empty;

        /// <summary>
        /// 其他
        /// </summary>
        public string Others { get; set; }

        /// <summary>
        /// 受害部位
        /// </summary>
        [Required]
        public List<TreePartEnum> DamagedPart { get; set; } = new();

        /// <summary>
        /// 受害部位
        /// </summary>
        public List<string> DamagedPartName => DamagedPart.Select(x => x.GetDescription()).ToList();

        /// <summary>
        /// 樹木高度
        /// </summary>
        public decimal TreeHeight { get; set; }

        /// <summary>
        /// 樹木直徑
        /// </summary>
        public decimal TreeDiameter { get; set; }

        /// <summary>
        /// 現地種植時間
        /// </summary>
        public int LocalPlantingTime { get; set; }

        /// <summary>
        /// 首次發現受害時間
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime FirstDiscoveryDate { get; set; }

        /// <summary>
        /// 受害症狀描述
        /// </summary>
        public string DamageDescription { get; set; } = string.Empty;

        /// <summary>
        /// 立地種類 1 = 公園、校園, 人行道 = 2, 花台內 = 3, 建築周邊 = 4, 林地 = 5, 苗圃 = 6, 農地 = 7 , 空地 = 8
        /// </summary>
        public List<LocationTypeEnum> LocationType { get; set; } = new();

        /// <summary>
        /// 立地種類 1 = 公園、校園, 人行道 = 2, 花台內 = 3, 建築周邊 = 4, 林地 = 5, 苗圃 = 6, 農地 = 7 , 空地 = 8
        /// </summary>
        public List<string> LocationTypeName => LocationType.Select(x => x.GetDescription()).ToList();

        /// <summary>
        /// 樹基部狀況 1 = 水泥面 = 2, 柏油面 = 3, 植被泥土面 (地表有草皮或鬆潤木) = 4, 花台內 = 5, 人工鋪面 (水泥面、柏油面以外) = 6
        /// </summary>
        public List<TreeBaseConditionEnum> BaseCondition { get; set; } = new();

        /// <summary>
        /// 樹基部狀況 1 = 水泥面 = 2, 柏油面 = 3, 植被泥土面 (地表有草皮或鬆潤木) = 4, 花台內 = 5, 人工鋪面 (水泥面、柏油面以外) = 6
        /// </summary>
        public List<string> BaseConditionName => BaseCondition.Select(x => x.GetDescription()).ToList();

        /// <summary>
        /// 圖片
        /// </summary>
        public List<CaseFileDto> Photo { get; set; } = new();

        /// <summary>
        /// 案件狀態
        /// </summary>
        public CaseStatusEnum CaseStatus { get; set; }

        /// <summary>
        /// 案件狀態
        /// </summary>
        public string CaseStatusName => CaseStatus.GetDescription();
    }
}
