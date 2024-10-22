using admin_backend.Enums;

namespace admin_backend.DTOs.FAQ
{
    public class UpdateFAQDto
    {
        /// <summary>
        /// 問題
        /// </summary>
        public string? Question { get; set; } = string.Empty;

        /// <summary>
        /// 答案
        /// </summary>
        public string? Answer { get; set; } = string.Empty;

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        public StatusEnum? Status { get; set; }
    }
}
