using admin_backend.Enums;
using CommonLibrary.DTOs;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.DamageType
{
    public class AddDamageTypeDto : SortDto
    {
        /// <summary>
        /// 危害類型
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        [Required]
        public StatusEnum Status { get; set; }
    }
}
