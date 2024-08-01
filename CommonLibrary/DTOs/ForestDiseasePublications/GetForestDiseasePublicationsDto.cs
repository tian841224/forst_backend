using CommonLibrary.Enums;

namespace CommonLibrary.DTOs.ForestDiseasePublications
{
    public class GetForestDiseasePublicationsDto
    {
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string? Keyword { get; set; } = string.Empty;

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟 
        /// </summary>
        public StatusEnum? Status { get; set; }
    }
}
