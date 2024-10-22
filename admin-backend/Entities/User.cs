using admin_backend.Enums;
using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.Entities
{
    /// <summary>
    /// 會員
    /// </summary>
    public class User : DefaultEntity
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        [Comment("帳號")]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        [Comment("密碼")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Required]
        [Comment("使用者名稱")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 狀態
        /// </summary>
        [Required]
        [Comment("狀態 0 = 關閉, 1 = 開啟, 2 = 停用")]
        public StatusEnum Status { get; set; }

        /// <summary>
        /// 最後登入時間
        /// </summary>
        [Comment("最後登入時間")]
        public DateTime LoginTime { get; set; }
    }
}
