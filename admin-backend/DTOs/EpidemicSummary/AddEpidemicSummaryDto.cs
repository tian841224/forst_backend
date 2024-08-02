using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.EpidemicSummary
{
    public class AddEpidemicSummaryDto
    {
        /// <summary>
        /// 標題
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 內容
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
