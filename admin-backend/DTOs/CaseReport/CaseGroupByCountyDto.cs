namespace admin_backend.DTOs.CaseReport
{
    public class CaseGroupByCountyDto
    {
        /// <summary>
        /// 縣市
        /// </summary>
        public string? County { get; set; }

        /// <summary>
        /// 開始日期
        /// </summary>
        public string? StartTime { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public string? EndTime { get; set; }
    }
}
