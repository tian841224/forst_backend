using admin_backend.Enums;

namespace admin_backend.DTOs.DamageType
{
    public class UpdateDamageTypeDto
    {
        /// <summary>
        /// 危害類型
        /// </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        public StatusEnum? Status { get; set; }
    }
}
