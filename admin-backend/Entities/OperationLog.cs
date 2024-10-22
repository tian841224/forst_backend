using admin_backend.Enums;
using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.Entities
{
    /// <summary>
    /// 系統操作紀錄
    /// </summary>
    public class OperationLog : DefaultEntity
    {
        /// <summary>
        /// 後台使用者ID
        /// </summary>
        [Comment("後台使用者ID")]
        public int? AdminUserId { get; set; }

        /// <summary>
        /// 使用者ID
        /// </summary>
        [Comment("使用者ID")]
        public int? UserId { get; set; }

        /// <summary>
        /// 異動類型
        /// </summary>
        [Required]
        [Comment("異動類型 新增 = 1, 指派 = 2, 編輯 = 3, 刪除 = 4")]
        public ChangeTypeEnum Type { get; set; }

        /// <summary>
        /// 異動內容
        /// </summary>
        [Required]
        [Comment("異動內容")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// IP
        /// </summary>
        [Required]
        [Comment("IP")]
        public string Ip { get; set; } = string.Empty;
    }
}
