using admin_backend.Enums;
using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace admin_backend.Entities
{
    /// <summary>
    /// 案件回覆
    /// </summary>
    public class CaseDiagnosisResult : DefaultEntity
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        [Comment("案件編號")]
        [Required]
        public int CaseId { get; set; }

        /// <summary>
        /// 送件方式
        /// </summary>
        [Comment("送件方式")]
        public List<SubmissionMethodEnum>? SubmissionMethod { get; set; } = new();

        /// <summary>
        /// 診斷方式
        /// </summary>
        [Comment("診斷方式")]
        public List<DiagnosisMethodEnum>? DiagnosisMethod { get; set; } = new();

        /// <summary>
        /// 危害狀況詳細描述
        /// </summary>
        [Comment("危害狀況詳細描述")]
        public string? HarmPatternDescription { get; set; } = string.Empty;

        /// <summary>
        /// 常見病蟲害
        /// </summary>
        [Comment("常見病蟲害")]
        public int? CommonDamageId { get; set; }

        /// <summary>
        /// 防治建議
        /// </summary>
        [Comment("防治建議")]
        public string? PreventionSuggestion { get; set; } = string.Empty;

        ///// <summary>
        ///// 防治建議圖片
        ///// </summary>
        //[Comment("防治建議圖片")]
        //public string PreventionSuggestionPhoto { get; set; }

        /// <summary>
        /// 危害病蟲名稱(舊)
        /// </summary>
        [Comment("危害病蟲名稱(舊)")]
        public string? OldCommonDamageName { get; set; } = string.Empty;

        /// <summary>
        /// 學名
        /// </summary>
        [Comment("學名")]
        public string? ScientificName { get; set; } = string.Empty;

        /// <summary>
        /// 呈報建議
        /// </summary>
        [Comment("呈報建議")]
        public string? ReportingSuggestion { get; set; } = string.Empty;

        ///// <summary>
        ///// 呈報建議圖片
        ///// </summary>
        //[Comment("呈報建議圖片")]
        //[Required]
        //public string ReportingSuggestionPhoto { get; set; } = string.Empty;

        /// <summary>
        /// 退回原因
        /// </summary>
        [Comment("退回原因")]
        public string? ReturnReason { get; set; } = string.Empty;

        [ForeignKey("CaseId")]
        public virtual CaseRecord Case { get; set; } = null!;

        [ForeignKey("CommonDamageId")]
        public virtual CommonDamage CommonDamage { get; set; } = null!;
    }
}
