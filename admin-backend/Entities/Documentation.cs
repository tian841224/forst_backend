using admin_backend.Enums;
using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.Entities
{
    /// <summary>
    /// 會員註冊使用條款
    /// </summary>
    public class Documentation : DefaultEntity
    {
        /// <summary>
        /// 使用條款類型
        /// </summary>
        [Required]
        [Comment("使用條款類型 1 = 同意書, 2 = 使用說明")]
        public DocumentationEnum Type { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        [Required]
        [Comment("內容")]
        public string Content { get; set; } = string.Empty;
    }
}
