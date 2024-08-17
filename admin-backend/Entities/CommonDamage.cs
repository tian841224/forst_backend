using admin_backend.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace admin_backend.Entities
{
    public class CommonDamage : SortDefaultEntity
    {
        /// <summary>
        /// 危害類型ID
        /// </summary>
        [Required]
        [Comment("危害類型ID")]
        public int DamageTypeId { get; set; }

        /// <summary>
        /// 危害種類ID
        /// </summary>
        [Required]
        [Comment("危害種類ID")]
        public int DamageClassId { get; set; }

        /// <summary>
        /// 病蟲危害名稱
        /// </summary>
        [Required]
        [Comment("病蟲危害名稱")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 危害部位
        /// </summary>
        [Required]
        [Comment("危害部位")]
        public List<DamagedPartEnum> DamagePart { get; set; } = new();

        /// <summary>
        /// 危害特徵
        /// </summary>
        [Required]
        [Comment("危害特徵")]
        public string DamageFeatures { get; set; } = string.Empty;

        /// <summary>
        /// 防治建議
        /// </summary>
        [Required]
        [Comment("危害類型ID")]
        public string Suggestions { get; set; } = string.Empty;

        /// <summary>
        /// 病蟲封面照片
        /// </summary>
        [Required]
        [Comment("危害類型ID")]
        public string Photo { get; set; } = string.Empty;

        /// <summary>
        /// 狀態
        /// </summary>
        [Required]
        [Comment("狀態 0 = 關閉, 1 = 開啟")]
        public StatusEnum Status { get; set; }

        [ForeignKey("DamageTypeId")]
        public virtual DamageType DamageType { get; set; } = null!;

        [ForeignKey("DamageClassId")]
        public virtual DamageClass DamageClass { get; set; } = null!;

    }
}
