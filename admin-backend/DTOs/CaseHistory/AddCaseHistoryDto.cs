using admin_backend.Enums;

namespace admin_backend.DTOs.CaseHistory
{
    public class AddCaseHistoryDto
    {
        /// <summary>
        /// 案件ID (外鍵)
        /// </summary>
        public int CaseId { get; set; }

        /// <summary>
        /// 操作時間
        /// </summary>
        public DateTime ActionTime { get; set; }

        /// <summary>
        /// 操作類型
        /// </summary>
        public ActionTypeEnum ActionType { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        public string? Description { get; set; }

        ///// <summary>
        ///// 操作人員身分
        ///// </summary>
        //[Required]
        //public int Role { get; set; }

        ///// <summary>
        ///// 操作人員
        ///// </summary>
        //[Required]
        //public int OperatorId { get; set; }
    }
}
