using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.User
{
    public class AddUserDto
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [EmailAddress]
        [Required]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        public string PKey { get; set; } = string.Empty;
    }
}
