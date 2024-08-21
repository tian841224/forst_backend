using admin_backend.Enums;

namespace admin_backend.DTOs.User
{
    public class UpdateUserDto
    {
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 舊密碼
        /// </summary>
        public string? OldKey { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string? pKey { get; set; }

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟, 2 = 停用
        /// </summary>
        public StatusEnum? Status { get; set; }
    }
}
