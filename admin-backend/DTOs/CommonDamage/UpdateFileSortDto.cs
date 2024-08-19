using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.CommonDamage
{
    public class UpdateFileSortDto
    {
        /// <summary>
        /// 檔案ID
        /// </summary>
        [Required]
        public int FileId { get; set; } 

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int Sort { get; set; }
    }
}
