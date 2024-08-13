using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.Login
{
    public class ResetPasswordDto
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
