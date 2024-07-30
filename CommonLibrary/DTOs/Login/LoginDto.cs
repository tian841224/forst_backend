using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.Login
{
    public class LoginDto
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
        public string Password { get; set; } = string.Empty;

        ///// <summary>
        ///// 驗證碼ID
        ///// </summary>
        //[Required]
        //public string CaptchaCode { get; set; } = string.Empty;

        ///// <summary>
        ///// 使用者輸入驗證碼
        ///// </summary>
        //[Required]
        //public string CaptchaUserInput { get; set; } = string.Empty;
    }
}
