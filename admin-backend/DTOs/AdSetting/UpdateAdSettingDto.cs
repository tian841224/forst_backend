using admin_backend.Enums;

namespace admin_backend.DTOs.AdSetting
{
    public class UpdateAdSettingDto
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 站台 1 = 林業自然保育署, 2 = 林業試驗所
        /// </summary>
        public List<WebsiteEnum>? Website { get; set; } = new List<WebsiteEnum>();

        /// <summary>
        /// PC圖案
        /// </summary>
        public IFormFile? PhotoPc { get; set; }

        /// <summary>
        /// 手機圖案
        /// </summary>
        public IFormFile? PhotoMobile { get; set; }

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        public StatusEnum? Status { get; set; }
    }
}
