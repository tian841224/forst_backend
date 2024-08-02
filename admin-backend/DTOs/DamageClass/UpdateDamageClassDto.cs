using admin_backend.Enums;
using CommonLibrary.DTOs;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.DamageClass
{
    public class UpdateDamageClassDto : BasicDto
    {
        /// <summary>
        /// 危害種類
        /// </summary>
        [Required]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 危害類型ID
        /// </summary>
        [Required]
        public int? DamageTypeId { get; set; }

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        [Required]
        public StatusEnum? Status { get; set; }
    }
}
