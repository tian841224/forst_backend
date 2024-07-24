using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Entities
{
    /// <summary>
    /// 廣告設定
    /// </summary>
    public class AdSetting : DefaultEntity
    {
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
        public WebsiteEnum Website { get; set; }

        /// <summary>
        /// 廣告位置
        /// </summary>
        [Required]
        [Comment("廣告位置 1 = 橫幅, 2 = 首頁")]
        public PositionEnum Position { get; set; }

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
    }
}
