﻿using CommonLibrary.Enums;
using CommonLibrary.Extensions; 
using System.Text.Json.Serialization;

namespace CommonLibrary.DTOs.AdminUser
{
    public class AdminUserResponse: DefaultResponseDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 照片
        /// </summary>
        public string Photo { get; set; } = string.Empty;

        /// <summary>
        /// 角色
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        public StatusEnum Status { get; set; }

        /// <summary>
        /// 登入時間
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime LoginTime { get; set; }
    }
}
