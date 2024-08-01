using CommonLibrary.DTOs.Common;

namespace CommonLibrary.DTOs.EpidemicSummary
{
    public class EpidemicSummaryResponse : DefaultResponseDto
    {
        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }
}
