using admin_backend.Enums;
using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.Entities
{
    /// <summary>
    /// 危害類型
    /// </summary>
    public class DamageType : SortDefaultEntity
    {
        /// <summary>
        /// 危害類型
        /// </summary>
        [Required]
        [Comment("危害類型")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 狀態
        /// </summary>
        [Comment("狀態 0 = 關閉, 1 = 開啟")]
        [Required]
        public StatusEnum Status { get; set; }
    }
}
