using admin_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.News
{
    public class AddNewsDto
    {
        ///// <summary>
        ///// 發佈者ID
        ///// </summary>
        //public int AdminUserId { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 公告類型 一般公告 = 1, 重要公告 = 2, 活動公告 = 3, 跑馬燈 = 4
        /// </summary>
        [Required]
        public AnnouncementEnum Type { get; set; }

        /// <summary>
        /// 發佈內容
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 置頂
        /// </summary>
        [Required]
        public bool Pinned { get; set; }

        /// <summary>
        /// 發佈網站 1 = 林業自然保育署, 2 = 林業試驗所
        /// </summary>
        [Required]
        public List<WebsiteEnum> WebsiteReleases { get; set; } = new();

        /// <summary>
        /// 是否開啟排程
        /// </summary>
        [Required]
        public bool Schedule { get; set; }

        /// <summary>
        /// 排程開始時間
        /// </summary>
        public string? StartTime { get; set; } = string.Empty;

        /// <summary>
        /// 排程結束時間
        /// </summary>
        public string? EndTime { get; set; } = string.Empty;

        /// <summary>
        /// 發佈狀態 0 = 未發佈, 1 = 發佈
        /// </summary>
        [Required]
        public StatusEnum Status { get; set; }
    }
}
