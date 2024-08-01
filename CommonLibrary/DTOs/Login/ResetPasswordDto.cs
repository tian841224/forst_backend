using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.Login
{
    public class ResetPasswordDto
    {
        /// <summary>
        /// 信箱
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
