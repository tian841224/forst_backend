namespace admin_backend.DTOs.CaseReport
{
    public class GroupByMonthResponse
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; } 

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; } 

        /// <summary>
        /// 數量
        /// </summary>
        public int Count { get; set; }
    }
}
