namespace admin_backend.DTOs.CaseReport
{
    public class GroupByCountyResponse
    {
        /// <summary>
        /// 縣市
        /// </summary>
        public string County { get; set; } = string.Empty;

        /// <summary>
        /// 數量
        /// </summary>
        public int Count { get; set; } 
    }
}
