namespace CommonLibrary.DTOs
{
    public class APIResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 資料
        /// </summary>
        public object? Date { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMsg { get; set; } = string.Empty;
    }
}
