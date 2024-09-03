using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using admin_backend.Enums;

namespace admin_backend.Entities
{
    public class CaseHistory:DefaultEntity
    {
        /// <summary>
        /// 案件ID (外鍵)
        /// </summary>
        [Required]
        [ForeignKey("CaseRecord")]
        public int CaseId { get; set; }

        /// <summary>
        /// 操作時間
        /// </summary>
        [Required]
        [Comment("操作時間")]
        public DateTime ActionTime { get; set; }

        /// <summary>
        /// 操作類型
        /// </summary>
        [Required]
        [Comment("操作類型")]
        public ActionTypeEnum ActionType { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        [Comment("說明")]
        public string? Description { get; set; }

        /// <summary>
        /// 操作人員身分
        /// </summary>
        [Required]
        public int Role {  get; set; }

        /// <summary>
        /// 操作人員
        /// </summary>
        [Required]
        public int OperatorId { get; set; }

        /// <summary>
        /// 案件
        /// </summary>
        public virtual CaseRecord CaseRecord { get; set; } = null!;
    }
}
