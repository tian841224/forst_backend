using admin_backend.Enums;
using CommonLibrary.Extensions;

namespace admin_backend.DTOs.CaseHistory
{
    public class CaseHistoryResponse
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
        /// 操作類型名稱
        /// </summary>
        public string ActionTypeName => ActionType.GetDescription();

        /// <summary>
        /// 說明
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 操作人員
        /// </summary>
        public string Operator { get; set; }
    }
}
