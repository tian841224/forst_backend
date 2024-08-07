using admin_backend.Enums;
using CommonLibrary.DTOs;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.ForestDiseasePublications
{
    public class AddForestDiseasePublicationsDto : SortDto
    {
        /// <summary>
        /// 出版品類型 林業叢刊 = 1, 相關摺頁 = 2
        /// </summary>
        [Required]
        public int Type { get; set; }

        /// <summary>
        /// 出版品名稱
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 出版品日期
        /// </summary>
        public DateTime? Date { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 出版品連結
        /// </summary>
        public string? Link { get; set; } = string.Empty;

        /// <summary>
        /// 出版單位
        /// </summary>
        public string? Unit { get; set; } = string.Empty;

        /// <summary>
        /// 出版品檔案
        /// </summary>
        [Required]
        public List<IFormFile?> File { get; set; } = new List<IFormFile?>();

        /// <summary>
        /// 出版品作者
        /// </summary>
        public List<string>? Authors { get; set; } = new ();

        /// <summary>
        /// 發佈狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        public StatusEnum Status { get; set; }
    }
}
