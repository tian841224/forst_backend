﻿using admin_backend.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.AdSetting
{
    public class AddAdSettingDto
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 站台 1 = 林業自然保育署, 2 = 林業試驗所
        /// </summary>
        [SwaggerParameter(Description = "站台 1 = 林業自然保育署, 2 = 林業試驗所")]
        [Required]
        public WebsiteEnum Website { get; set; }

        /// <summary>
        /// 廣告位置 1 = 橫幅, 2 = 首頁
        /// </summary>
        [SwaggerParameter(Description = "廣告位置 1 = 橫幅, 2 = 首頁")]
        [Required]
        public PositionEnum Position { get; set; }

        /// <summary>
        /// PC圖案
        /// </summary>
        public IFormFile? PhotoPc { get; set; }

        /// <summary>
        /// 手機圖案
        /// </summary>

        public IFormFile? PhotoMobile { get; set; }

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        [SwaggerParameter(Description = "狀態 0 = 關閉, 1 = 開啟")]
        [Required]
        public StatusEnum Status { get; set; }

    }
}
