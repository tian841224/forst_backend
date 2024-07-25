using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibrary.Entities
{
    /// <summary>
    /// 後台帳號
    /// </summary>
    public class AdminUser : DefaultEntity
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        [Comment("帳號")]
        [StringLength(100)]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        [Comment("密碼")]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 信箱
        /// </summary>
        [Required]
        [Comment("信箱")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 照片
        /// </summary>
        [Comment("照片")]
        public string Photo { get; set; } = string.Empty;

        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        [Comment("角色")]
        public int RoleId { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        [Required]
        [Comment("狀態 0 = 關閉, 1 = 開啟")]
        public StatusEnum Status { get; set; }

        /// <summary>
        /// 登入時間
        /// </summary>
        [Comment("登入時間")]
        public DateTime LoginTime { get; set; } 

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } 
    }
}
