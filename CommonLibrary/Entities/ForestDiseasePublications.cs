using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Entities
{
    /// <summary>
    /// 林木疫情出版品
    /// </summary>
    public class ForestDiseasePublications : DefaultEntity
    {
        /// <summary>
        /// 出版品類型
        /// </summary>
        [Comment("出版品類型 林業叢刊 = 1, 相關摺頁 = 2")]
        public int Type { get; set; } 

        /// <summary>
        /// 出版品名稱
        /// </summary>
        [Required]
        [Comment("出版品名稱")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 出版品日期
        /// </summary>
        [Comment("出版品日期")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 出版品連結
        /// </summary>
        [Comment("出版品連結")]
        public string Link { get; set; } = string.Empty;

        /// <summary>
        /// 出版單位
        /// </summary>
        [Comment("出版單位")]
        [StringLength(100)]
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// 出版品檔案
        /// </summary>
        [Required]
        [Comment("出版品檔案")]
        public string File { get; set; } = string.Empty;

        /// <summary>
        /// 出版品作者
        /// </summary>
        [Comment("出版品作者")]
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// 發佈狀態
        /// </summary>
        [Required]
        [Comment("狀態 0 = 關閉, 1 = 開啟")]
        public StatusEnum Status { get; set; }
    }
}
