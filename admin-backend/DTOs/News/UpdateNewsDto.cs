using admin_backend.Enums;

namespace admin_backend.DTOs.News
{
    public class UpdateNewsDto
    {
        /// <summary>
        /// 標題
        /// </summary>
        public string? Title { get; set; } = string.Empty;

        /// <summary>
        /// 公告類型 一般公告 = 1, 重要公告 = 2, 活動公告 = 3, 跑馬燈 = 4
        /// </summary>
        public AnnouncementEnum? Type { get; set; }

        /// <summary>
        /// 發佈內容
        /// </summary>
        public string? Content { get; set; } = string.Empty;

        /// <summary>
        /// 置頂
        /// </summary>
        public bool? Pinned { get; set; }

        /// <summary>
        /// 發佈網站 1 = 林業自然保育署, 2 = 林業試驗所
        /// </summary>
        public List<WebsiteEnum>? WebsiteReleases { get; set; } = new();

        /// <summary>
        /// 是否開啟排程
        /// </summary>
        public bool? Schedule { get; set; }

        /// <summary>
        /// 排程開始時間
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 排程結束時間
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 發佈狀態 0 = 未發佈, 1 = 發佈
        /// </summary>
        public StatusEnum? Status { get; set; }
    }
}
