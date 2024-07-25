using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibrary.Entities
{
    /// <summary>
    /// 常見病蟲害
    /// </summary>
    public class CommonPest : DefaultEntity
    {
        /// <summary>
        /// 危害類型
        /// </summary>
        [Required]
        [Comment("危害類型")]
        public int DamageTypeId { get; set; }

        /// <summary>
        /// 危害種類
        /// </summary>
        [Required]
        [Comment("危害種類")]
        public int DamageClassId { get; set; }

        /// <summary>
        /// 病蟲危害名稱
        /// </summary>
        [Required]
        [StringLength(100)]
        [Comment("病蟲危害名稱")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 危害部位
        /// </summary>
        [Required]
        [Comment("危害部位 2 = 侵害土壤部, 3 = 樹幹, 5 = 樹枝, 6 = 樹葉, 7 = 花, 9 = 全面異常症狀病害")]
        public List<TreePartEnum> DamagePart { get; set; } = new();

        /// <summary>
        /// 危害特徵
        /// </summary>
        [Required]
        [Comment("危害特徵")]
        public string DamageCharacteristics { get; set; } = string.Empty;

        /// <summary>
        /// 防治建議
        /// </summary>
        [Required]
        [Comment("防治建議")]
        public string ControlRecommendations { get; set; } = string.Empty;

        /// <summary>
        /// 病蟲封面圖片
        /// </summary>
        [Required]
        [Comment("病蟲封面圖片")]
        public string Photo { get; set; } = string.Empty;

        /// <summary>
        /// 狀態
        /// </summary>
        [Comment("狀態 0 = 關閉, 1 = 開啟")]
        [Required]
        public StatusEnum Status { get; set; }

        /// <summary>
        /// 下架日期
        /// </summary>
        [Comment("下架日期")]
        public DateTime UnpublishDate { get; set; }

        [ForeignKey("DamageClassId")]
        public virtual DamageClass DamageClass { get; set; } 

        [ForeignKey("DamageTypeId")]
        public virtual DamageType DamageType { get; set; }
    }
}
