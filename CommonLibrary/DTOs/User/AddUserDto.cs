namespace CommonLibrary.DTOs.User
{
    public class AddUserDto
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
