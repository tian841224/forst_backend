using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibrary.Entities
{
    public class DiagnosisResult : DefaultEntity
    {
        /// <summary>
        /// 送件方式
        /// </summary>
        [Required]
        [StringLength(500)]
        [Comment("送件方式 1 = 線上, 2 = email, 3 = 自送, 4 = 郵寄, 電話諮詢 = 5")]
        public CommunicationMethodEnum CommunicationMethod { get; set; } 

        /// <summary>
        /// 診斷方式
        /// </summary>
        [Required]
        [StringLength(500)]
        [Comment("診斷方式")]
        public string DiagnosisMethod { get; set; } = string.Empty;

        /// <summary>
        /// 當前情況/環境描述
        /// </summary>
        [Required]
        [Comment("當前情況/環境描述")]
        public string CurrentCondition { get; set; } = string.Empty;

        /// <summary>
        /// 危害種類
        /// </summary>
        [Required]
        [Comment("危害種類")]
        public int DamageClassId { get; set; }

        /// <summary>
        /// 危害種類
        /// </summary>
        [Required]
        [Comment("危害類型")]
        public int DamageTypeId { get; set; }
        
        /// <summary>
        /// 病蟲害名稱
        /// </summary>
        [Required]
        [StringLength(200)]
        [Comment("病蟲害名稱")]
        public int CommonPestId { get; set; }

        /// <summary>
        /// 防治建議
        /// </summary>
        [Required]
        [Comment("防治建議")]
        public string PreventionMeasures { get; set; } = string.Empty;

        /// <summary>
        /// 異狀描述
        /// </summary>
        [Required]
        [Comment("異狀描述")]
        public string SymptomDescription { get; set; } = string.Empty;

        /// <summary>
        /// 是否已送出
        /// </summary>
        [Required]
        [Comment("是否已送出")]
        public bool IsSubmitted { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } 

    }
}
