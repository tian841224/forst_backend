using admin_backend.Enums;
using CommonLibrary.DTOs;
using System.Text.Json.Serialization;
using CommonLibrary.Extensions;

namespace admin_backend.DTOs.ForestDiseasePublications
{
    public class ForestDiseasePublicationsResponse : SortDefaultResponseDto
    {
        /// <summary>
        /// 出版品類型 林業叢刊 = 1, 相關摺頁 = 2
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 出版品類型 林業叢刊 = 1, 相關摺頁 = 2
        /// </summary>
        public string TypeName { get; set; } = string.Empty;

        /// <summary>
        /// 出版品名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 出版品日期
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// 出版品連結
        /// </summary>
        public string Link { get; set; } = string.Empty;

        /// <summary>
        /// 出版單位
        /// </summary>
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// 出版品檔案
        /// </summary>
        public List<ForestDiseasePublicationsFileDto> File { get; set; } = new();

        /// <summary>
        /// 出版品作者
        /// </summary>
        public List<string> Authors { get; set; } = new();

        /// <summary>
        /// 發佈狀態
        /// </summary>
        public StatusEnum Status { get; set; }
    }
}
