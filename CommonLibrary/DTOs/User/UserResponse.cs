using CommonLibrary.Enums;
using System.Text.Json.Serialization;
using CommonLibrary.Extensions;
using CommonLibrary.DTOs.Common;

namespace CommonLibrary.DTOs.User
{
    public class UserResponse : DefaultResponseDto
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟, 2 = 停用
        /// </summary>
        public StatusEnum Status { get; set; }

        /// <summary>
        /// 最後登入時間
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime LoginTime { get; set; }
    }
}
