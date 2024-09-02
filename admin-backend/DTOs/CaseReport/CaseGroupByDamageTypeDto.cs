namespace admin_backend.DTOs.CaseReport
{
    public class CaseGroupByDamageTypeDto
    {
        /// <summary>
        /// 危害種類類別
        /// </summary>
        public int? DamageTypeId { get; set; }

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
