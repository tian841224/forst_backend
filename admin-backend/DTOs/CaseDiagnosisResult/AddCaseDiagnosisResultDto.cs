using admin_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.CaseDiagnosisResult
{
    public class AddCaseDiagnosisResultDto
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        [Required]
        public int CaseId { get; set; }

        /// <summary>
        /// 送件方式
        /// </summary>
        [Required]
        public List<SubmissionMethodEnum> SubmissionMethod { get; set; } = new();

        /// <summary>
        /// 診斷方式
        /// </summary>
        [Required]
        public List<DiagnosisMethodEnum> DiagnosisMethod { get; set; } = new();

        /// <summary>
        /// 危害狀況詳細描述
        /// </summary>
        [Required]
        public string HarmPatternDescription { get; set; } = string.Empty;

        /// <summary>
        /// 常見病蟲害
        /// </summary>
        [Required]
        public int CommonDamageId { get; set; }

        /// <summary>
        /// 防治建議
        /// </summary>
        [Required]
        public string PreventionSuggestion { get; set; } = string.Empty;

        ///// <summary>
        ///// 防治建議圖片
        ///// </summary>
        //[Comment("防治建議圖片")]
        //public string PreventionSuggestionPhoto { get; set; }

        /// <summary>
        /// 危害病蟲名稱(舊)
        /// </summary>
        public string OldCommonDamageName { get; set; } = string.Empty;

        /// <summary>
        /// 學名
        /// </summary>
        public string ScientificName { get; set; } = string.Empty;

        /// <summary>
        /// 呈報建議
        /// </summary>
        [Required]
        public string ReportingSuggestion { get; set; } = string.Empty;

        ///// <summary>
        ///// 呈報建議圖片
        ///// </summary>
        //[Required]
        //public List<IFormFile> Photo { get; set; } = new();

        ///// <summary>
        ///// 退回原因
        ///// </summary>
        //public string? ReturnReason { get; set; } 
    }
}
