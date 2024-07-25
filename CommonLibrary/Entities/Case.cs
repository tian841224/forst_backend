using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibrary.Entities
{
    public class Case : DefaultEntity
    {
        /// <summary>
        /// 指派人
        /// </summary>
        [Comment("指派人")]
        public int AdminUserId { get; set; } 

        /// <summary>
        /// 單位名稱
        /// </summary>
        [Comment("單位名稱")]
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 地址
        /// </summary>
        [Required]
        [Comment("地址")]
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
        [Comment("Email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 受害樹木地址
        /// </summary>
        [Required]
        [Comment("受害樹木地址")]
        public string CaseAddress { get; set; } = string.Empty;

        /// 林班地位置

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
        /// 受害部位
        /// </summary>
        [Required]
        [Comment("受害部位 1 = 根, 莖、4 = 枝條, 6 = 樹葉, 7 = 花果, 8 = 全株")]
        public TreePartEnum DamagedPart { get; set; }

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
        public LocationTypeEnum LocationType { get; set; }

        /// <summary>
        /// 樹基部狀況
        /// </summary>
        [Required]
        [Comment("樹基部狀況 1 = 水泥面 = 2, 柏油面 = 3, 植被泥土面 (地表有草皮或鬆潤木) = 4, 花台內 = 5, 人工鋪面 (水泥面、柏油面以外) = 6")]
        public TreeBaseConditionEnum BaseCondition { get; set; }

        /// <summary>
        /// 圖片
        /// </summary>
        [Comment("圖片")]
        public string Photo { get; set; } = string.Empty;

        [ForeignKey("AdminUserId")]
        public virtual AdminUser AdminUser { get; set; } 

        [ForeignKey("TreeBasicInfoId")]
        public virtual TreeBasicInfo TreeBasicInfo { get; set; }
    }
}
