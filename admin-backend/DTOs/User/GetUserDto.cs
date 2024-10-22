using admin_backend.Enums;
using CommonLibrary.DTOs;

namespace admin_backend.DTOs.User
{
    public class GetUserDto 
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string? Account { get; set; } = string.Empty;

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟, 2 = 停用
        /// </summary>
        public StatusEnum? Status { get; set; }

        /// <summary>
        /// 最後登入時間
        /// </summary>
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; } = new PagedOperationDto();
    }
}
