using admin_backend.Enums;
using CommonLibrary.DTOs;

namespace admin_backend.DTOs.News
{
    public class GetNewsDto
    {
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string? Keyword { get; set; } = string.Empty;

        /// <summary>
        /// 公告類型 一般公告 = 1, 重要公告 = 2, 活動公告 = 3, 跑馬燈 = 4
        /// </summary>
        public AnnouncementEnum? Type { get; set; }

        /// <summary>
        /// 發佈網站 1 = 林業自然保育署, 2 = 林業試驗所
        /// </summary>
        public WebsiteEnum? WebsiteReleases { get; set; } 

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        public StatusEnum? Status { get; set; }

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; } = new PagedOperationDto();
    }
}
