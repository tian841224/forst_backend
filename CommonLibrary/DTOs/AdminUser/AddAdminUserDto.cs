using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.AdminUser
{
    public class AddAdminUserDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [Comment("姓名")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 信箱
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 照片
        /// </summary>
        public string? Photo { get; set; } 

        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟 
        /// </summary>
        [Required]
        public StatusEnum Status { get; set; }
    }
}
