using CommonLibrary.Enums;

namespace CommonLibrary.DTOs.OperationLog
{
    public class GetOperationLogResponseDto
    {
        public int Id { get; set; }

        /// <summary>
        /// 使用者ID
        /// </summary>
        public int AdminUserId { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string AdminUserIdName { get; set; } = string.Empty;

        /// <summary>
        /// 異動類型 新增 = 1, 指派 = 2, 編輯 = 3, 刪除 = 4
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 異動內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; } = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
