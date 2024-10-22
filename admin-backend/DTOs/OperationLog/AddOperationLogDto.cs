using admin_backend.Enums;

namespace admin_backend.DTOs.OperationLog
{
    public class AddOperationLogDto
    {
        ///// <summary>
        ///// 使用者ID
        ///// </summary>
        //public int AdminUserId { get; set; }

        /// <summary>
        /// 異動類型 新增 = 1, 指派 = 2, 編輯 = 3, 刪除 = 4
        /// </summary>
        public ChangeTypeEnum Type { get; set; }

        /// <summary>
        /// 異動內容
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }
}
