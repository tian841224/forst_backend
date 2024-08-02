using admin_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.Documentation
{
    public class AddDocumentationDto
    {
        /// <summary>
        /// 使用條款類型 1 = 同意書, 2 = 使用說明
        /// </summary>
        [Required]
        public DocumentationEnum Type { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
