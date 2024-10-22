using admin_backend.Enums;
using CommonLibrary.DTOs;

namespace admin_backend.DTOs.FAQ
{
    public class GetFAQDto
    {
        /// <summary>
        /// 問題
        /// </summary>
        public string? Question { get; set; } = string.Empty;

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
