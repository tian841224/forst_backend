﻿using admin_backend.Enums;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.DTOs.MailConfig
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
        public int Port { get; set; }

        /// <summary>
        /// 加密方式 1 = SSL, 2 = TSL
        /// </summary>
        public EncryptedEnum Encrypted { get; set; }

        public string EncryptedName => Encrypted.GetDescription();

        /// <summary>
        /// 寄信帳號
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 寄信密碼
        /// </summary>
        public string Pkey { get; set; } = string.Empty;

        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
