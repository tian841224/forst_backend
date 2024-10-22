using admin_backend.Enums;
using CommonLibrary.DTOs;

namespace admin_backend.DTOs.AdminUser
{
    public class GetAdminUserDto 
    {
        public int? Id { get; set; }

        /// <summary>
        /// 關鍵字
        /// </summary>
        public string? Keyword { get; set; } = string.Empty;

        /// <summary>
        /// 身分
        /// </summary>
        public int? RoleId { get; set; }

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
