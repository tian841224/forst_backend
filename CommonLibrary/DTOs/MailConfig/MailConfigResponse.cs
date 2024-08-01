using CommonLibrary.DTOs.Common;
using CommonLibrary.Enums;

namespace CommonLibrary.DTOs.MailConfig
{
    public class MailConfigResponse : DefaultResponseDto
    {
        /// <summary>
        /// 主機
        /// </summary>
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// Port
        /// </summary>
        public byte Port { get; set; }

        /// <summary>
        /// 加密方式 1 = SSL, 2 = TSL
        /// </summary>
        public EncryptedEnum Encrypted { get; set; }

        /// <summary>
        /// 寄信帳號
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 寄信密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
