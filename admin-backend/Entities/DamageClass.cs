using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using admin_backend.Enums;
using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace admin_backend.Entities
{
    /// <summary>
    /// 危害種類
    /// </summary>
    public class DamageClass : DefaultEntity
    {
        /// <summary>
        /// 危害種類
        /// </summary>
        [Required]
        [Comment("危害種類")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 危害類型ID
        /// </summary>
        [Required]
        [Comment("危害類型ID")]
        public int DamageTypeId { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        [Required]
        [Comment("狀態 0 = 關閉, 1 = 開啟")]
        public StatusEnum Status { get; set; }

        [ForeignKey("DamageTypeId")]
        public virtual DamageType DamageType { get; set; } = null!;
    }
}
