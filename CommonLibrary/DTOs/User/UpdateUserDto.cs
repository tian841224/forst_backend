using CommonLibrary.Enums;

namespace CommonLibrary.DTOs.User
{
    public class UpdateUserDto
    {
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟, 2 = 停用
        /// </summary>
        public StatusEnum? Status { get; set; }
    }
}
