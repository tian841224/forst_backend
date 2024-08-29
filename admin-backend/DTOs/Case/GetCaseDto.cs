using admin_backend.Enums;
using CommonLibrary.DTOs;

namespace admin_backend.DTOs.Case
{
    public class GetCaseDto
    {
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string? Keyword { get; set; } = string.Empty;

        /// <summary>
        /// 開始日期
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 案件狀態
        /// </summary>
        public CaseStatusEnum? CaseStatus { get; set; }

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; } = new PagedOperationDto();
    }
}
