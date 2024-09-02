using admin_backend.Enums;
using CommonLibrary.DTOs;

namespace admin_backend.DTOs.Case
{
    public class GetCaseRecordDto
    {
        ///// <summary>
        ///// 關鍵字
        ///// </summary>
        //public string? Keyword { get; set; } = string.Empty;

        /// <summary>
        /// 指派人ID
        /// </summary>
        public int? AdminUserId { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public int? CaseNumber { get; set; }

        /// <summary>
        /// 案件開始日期
        /// </summary>
        public string? Case_StartTime { get; set; }

        /// <summary>
        /// 案件結束日期
        /// </summary>
        public string? Case_EndTime { get; set; }

        /// <summary>
        /// 危害類型
        /// </summary>
        public int? DamageType { get; set; }

        /// <summary>
        /// 危害種類
        /// </summary>
        public int? DamageClass { get; set; }

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
