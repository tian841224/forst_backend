using admin_backend.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace admin_backend.Entities
{
    /// <summary>
    /// 廣告設定
    /// </summary>
    public class AdSetting : DefaultEntity
    {
        /// <summary>
        /// 發佈者
        /// </summary>
        [Required]
        [Comment("發佈者")]
        public int AdminUserId { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        [Comment("名稱")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 站台
        /// </summary>
        [Required]
        [Comment("站台 1 = 林業自然保育署, 2 = 林業試驗所")]
        public List<WebsiteEnum> Website { get; set; } = new List<WebsiteEnum>();

        /// <summary>
        /// PC圖片
        /// </summary>
        [Comment("PC圖片")]
        public string? PhotoPc { get; set; }

        /// <summary>
        /// 手機圖片
        /// </summary>
        [Comment("手機圖片")]
        public string? PhotoMobile { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        [Required]
        [Comment("狀態 0 = 關閉, 1 = 開啟")]
        public StatusEnum Status { get; set; }

        [ForeignKey("AdminUserId")]
        public virtual AdminUser AdminUser { get; set; } = null!;
    }
}
