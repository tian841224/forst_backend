namespace admin_backend.DTOs.CaseReport
{
    public class GroupByCountyDamageResponse
    {
        /// <summary>
        /// 縣市
        /// </summary>
        public string County { get; set; } = string.Empty;

        /// <summary>
        /// 總數
        /// </summary>
        public int Count { get; set; }
    }
}
