using admin_backend.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.Entities
{
    /// <summary>
    /// 權限管理
    /// </summary>
    public class Permission : DefaultEntity
    {
        /// <summary>
        /// 權限名稱
        /// </summary>
        [Comment("權限名稱")]
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 權限類型
        /// </summary>
        [Comment("檢視 = 1, 新增 = 2, 指派 = 3, 編輯 = 4, 刪除 = 5")]
        [Required]
        public PermissionEnum Type { get; set; }
    }
}
