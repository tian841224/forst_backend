using admin_backend.Enums;
using CommonLibrary.DTOs;

namespace admin_backend.DTOs.OperationLog
{
    public class GetOperationLogDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// 帳號ID
        /// </summary>
        public int? AdminUserId { get; set; }

        /// <summary>
        /// 異動類型 新增 = 1, 指派 = 2, 編輯 = 3, 刪除 = 4
        /// </summary>
        public ChangeTypeEnum? Type { get; set; }

        /// <summary>
        /// 起始時間
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; }
    }
}
