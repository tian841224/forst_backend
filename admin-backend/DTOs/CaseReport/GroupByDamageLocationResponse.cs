namespace admin_backend.DTOs.CaseReport
{
    public class GroupByDamageLocationResponse
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
