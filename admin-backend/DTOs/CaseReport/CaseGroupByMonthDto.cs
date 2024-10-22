namespace admin_backend.DTOs.CaseReport
{
    public class CaseGroupByMonthDto
    {
        /// <summary>
        ///  null And 0 = 月、1 = 年
        /// </summary>
        public int? Type { get; set; }

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
