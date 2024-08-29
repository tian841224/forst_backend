using admin_backend.Enums;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.DTOs.CaseDiagnosisResult
{
    public class CaseDiagnosisResultResponse : DefaultResponseDto
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        public int CaseId { get; set; }

        /// <summary>
        /// 送件方式
        /// </summary>
        public List<SubmissionMethodEnum> SubmissionMethod { get; set; } = new();

        /// <summary>
        /// 送件方式
        /// </summary>
        public List<string> SubmissionMethodName => SubmissionMethod.Select(x => x.GetDescription()).ToList();

        /// <summary>
        /// 診斷方式
        /// </summary>
        public List<DiagnosisMethodEnum> DiagnosisMethod { get; set; } = new();

        /// <summary>
        /// 診斷方式
        /// </summary>
        public List<string> DiagnosisMethodName => DiagnosisMethod.Select(x => x.GetDescription()).ToList();

        /// <summary>
        /// 危害狀況詳細描述
        /// </summary>
        public string HarmPatternDescription { get; set; } = string.Empty;

        /// <summary>
        /// 常見病蟲害
        /// </summary>
        public int CommonDamageId { get; set; }

        /// <summary>
        /// 常見病蟲害
        /// </summary>
        public string? CommonDamageName { get; set; }

        /// <summary>
        /// 危害類型
        /// </summary>
        public string? DamageTypeName { get; set; } 

        /// <summary>
        /// 危害種類
        /// </summary>
        public string? DamageClassName { get; set; } 

        /// <summary>
        /// 防治建議
        /// </summary>
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
        public string ReportingSuggestion { get; set; } = string.Empty;

        ///// <summary>
        ///// 呈報建議圖片
        ///// </summary>
        //public string ReportingSuggestionPhoto { get; set; } = string.Empty;

        /// <summary>
        /// 退回原因
        /// </summary>
        public string ReturnReason { get; set; } = string.Empty;
    }
}
