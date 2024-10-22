﻿using admin_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.MailConfig
{
    public class AddMailConfigDto
    {
        /// <summary>
        /// 主機
        /// </summary>
        [Required]
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// Port
        /// </summary>
        [Required]
        public int Port { get; set; }

        /// <summary>
        /// 加密方式 1 = SSL, 2 = TSL
        /// </summary>
        [Required]
        public EncryptedEnum Encrypted { get; set; }

        /// <summary>
        /// 寄信帳號
        /// </summary>
        [Required]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 寄信密碼
        /// </summary>
        [Required]
        public string pKey { get; set; } = string.Empty;

        /// <summary>
        /// 顯示名稱
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
