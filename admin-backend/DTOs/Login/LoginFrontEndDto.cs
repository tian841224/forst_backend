using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.Login
{
    public class LoginFrontEndDto
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        public string PKey { get; set; } = string.Empty;
    }
}
