using admin_backend.Enums;

namespace admin_backend.DTOs.CaseDiagnosisResult
{
    public class UpdateCaseDiagnosisResultDto
    {
        /// <summary>
        /// 送件方式
        /// </summary>
        public List<SubmissionMethodEnum>? SubmissionMethod { get; set; }

        /// <summary>
        /// 診斷方式
        /// </summary>
        public List<DiagnosisMethodEnum>? DiagnosisMethod { get; set; }

        /// <summary>
        /// 危害狀況詳細描述
        /// </summary>
        public string? HarmPatternDescription { get; set; }

        /// <summary>
        /// 常見病蟲害
        /// </summary>
        public int? CommonDamageId { get; set; }

        /// <summary>
        /// 防治建議
        /// </summary>
        public string? PreventionSuggestion { get; set; }

        ///// <summary>
        ///// 防治建議圖片
        ///// </summary>
        //[Comment("防治建議圖片")]
        //public string PreventionSuggestionPhoto { get; set; }

        /// <summary>
        /// 危害病蟲名稱(舊)
        /// </summary>
        public string? OldCommonDamageName { get; set; }

        /// <summary>
        /// 學名
        /// </summary>
        public string? ScientificName { get; set; }

        /// <summary>
        /// 呈報建議
        /// </summary>
        public string? ReportingSuggestion { get; set; }

        ///// <summary>
        ///// 呈報建議圖片
        ///// </summary>
        //[Required]
        //public List<IFormFile>? ReportingSuggestionPhoto { get; set; } = new();

        /// <summary>
        /// 退回原因
        /// </summary>
        public string? ReturnReason { get; set; }
    }
}
