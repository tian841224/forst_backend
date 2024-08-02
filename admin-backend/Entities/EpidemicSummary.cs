using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.Entities
{
    /// <summary>
    /// 疫情簡介
    /// </summary>
    public class EpidemicSummary : DefaultEntity
    {
        /// <summary>
        /// 標題
        /// </summary>
        [Comment("標題")]
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 內容
        /// </summary>
        [Comment("內容")]
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
