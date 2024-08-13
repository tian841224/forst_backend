using admin_backend.Enums;
using CommonLibrary.DTOs;

namespace admin_backend.DTOs.AdSetting
{
    public class AdSettingResponse : DefaultResponseDto
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 站台 1 = 林業自然保育署, 2 = 林業試驗所
        /// </summary>
        public WebsiteEnum Website { get; set; }

        /// <summary>
        /// 廣告位置 1 = 橫幅, 2 = 首頁
        /// </summary>
        public PositionEnum Position { get; set; }

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
