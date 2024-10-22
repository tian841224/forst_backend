using admin_backend.Enums;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.DTOs.AdSetting
{
    public class AdSettingResponse : DefaultResponseDto
    {
        /// <summary>
        /// 發佈者ID
        /// </summary>
        public int AdminUserId { get; set; }

        /// <summary>
        /// 發佈者
        /// </summary>
        public string AdminUserName { get; set; } = string.Empty;

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 站台 1 = 林業自然保育署, 2 = 林業試驗所
        /// </summary>
        public List<WebsiteEnum> Website { get; set; } = new();

        /// <summary>
        /// 站台名稱
        /// </summary>
        public List<string> WebsiteName => Website.Select(w => w.GetDescription()).ToList();

        /// <summary>
        /// PC圖片
        /// </summary>
        public string? PhotoPc { get; set; }

        /// <summary>
        /// 手機圖片
        /// </summary>
        public string? PhotoMobile { get; set; }

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        public StatusEnum Status { get; set; }
    }
}
