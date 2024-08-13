using admin_backend.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.Entities
{
    /// <summary>
    /// 郵寄信件設定
    /// </summary>
    public class MailConfig : DefaultEntity
    {
        /// <summary>
        /// 主機
        /// </summary>
        [Required]
        [Comment("主機")]
        [StringLength(100)]
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// Port
        /// </summary>
        [Required]
        [Comment("Port")]
        public int Port { get; set; }

        /// <summary>
        /// 加密方式
        /// </summary>
        [Required]
        [Comment("加密方式 1 = SSL, 2 = TSL")]
        public EncryptedEnum Encrypted { get; set; }

        /// <summary>
        /// 寄信帳號
        /// </summary>
        [Required]
        [Comment("寄信帳號")]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 寄信密碼
        /// </summary>
        [Required]
        [Comment("寄信密碼")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 顯示名稱
        /// </summary>
        [Required]
        [Comment("顯示名稱")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
