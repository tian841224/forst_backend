using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Entities
{
    /// <summary>
    /// 常見問答
    /// </summary>
    public class FAQ : DefaultEntity
    {
        /// <summary>
        /// 問題
        /// </summary>
        [Required]
        [Comment("問題")]
        public string Question { get; set; } = string.Empty;

        /// <summary>
        /// 答案
        /// </summary>
        [Required]
        [Comment("答案")]
        public string Answer { get; set; } = string.Empty;

        /// <summary>
        /// 狀態
        /// </summary>
        [Required]
        [Comment("狀態 0 = 關閉, 1 = 開啟")]
        public StatusEnum Status { get; set; }
    }
}
