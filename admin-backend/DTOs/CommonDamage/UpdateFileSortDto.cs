using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.CommonDamage
{
    public class UpdateFileSortDto
    {
        /// <summary>
        /// 檔案名稱
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int Sort { get; set; }
    }
}
