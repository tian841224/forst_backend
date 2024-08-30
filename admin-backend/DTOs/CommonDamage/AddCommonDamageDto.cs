using admin_backend.DTOs.DamageType;
using admin_backend.Enums;
using CommonLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.CommonDamage
{
    public class AddCommonDamageDto : SortDto
    {
        /// <summary>
        /// 危害類型ID
        /// </summary>
        [Required]
        public int DamageTypeId { get; set; }

        /// <summary>
        /// 危害種類ID
        /// </summary>
        [Required]
        public int DamageClassId { get; set; }

        /// <summary>
        /// 病蟲危害名稱
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 危害部位
        /// </summary>
        [Required]
        [FromForm]
        public List<DamagedPartEnum> DamagePart { get; set; } = null!;

        /// <summary>
        /// 危害特徵
        /// </summary>
        [Required]
        public string DamageFeatures { get; set; } = string.Empty;

        /// <summary>
        /// 防治建議
        /// </summary>
        [Required]
        public string Suggestions { get; set; } = string.Empty;

        /// <summary>
        /// 病蟲封面照片
        /// </summary>
        public List<CommonDamagePhotoDto>? File { get; set; }

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        [Required]
        public StatusEnum Status { get; set; }
    }
}
