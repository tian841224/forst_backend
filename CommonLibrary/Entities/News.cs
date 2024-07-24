using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Entities
{
    /// <summary>
    /// 最新消息
    /// </summary>
    public class News : DefaultEntity
    {
        /// 標題
        /// </summary>
        [Required]
        [Comment("標題")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 公告類型
        /// </summary>
        [Required]
        [Comment("公告類型 一般公告 = 1, 重要公告 = 2, 活動公告 = 3, 跑馬燈 = 4")]
        public AnnouncementEnum Type { get; set; }

        /// <summary>
        /// 發佈內容
        /// </summary>
        [Required]
        [Comment("發佈內容")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 置頂
        /// </summary>
        [Required]
        [Comment("置頂")]
        public bool Pinned { get; set; }

        /// <summary>
        /// 發佈網站
        /// </summary>
        [Required]
        [Comment("發佈網站")]
        public WebsiteEnum WebsiteReleases { get; set; }

        /// <summary>
        /// 是否開啟排程
        /// </summary>
        [Required]
        [Comment("是否開啟排程")]
        public bool Schedule { get; set; }

        /// <summary>
        /// 排程開始時間
        /// </summary>
        [Comment("排程開始時間")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 排程結束時間
        /// </summary>
        [Comment("排程結束時間")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 發佈狀態
        /// </summary>
        [Required]
        [Comment("發佈狀態 0 = 未發佈, 1 = 發佈")]
        public StatusEnum Status { get; set; }
    }
}
