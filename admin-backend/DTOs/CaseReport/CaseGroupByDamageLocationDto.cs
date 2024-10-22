namespace admin_backend.DTOs.CaseReport
{
    public class CaseGroupByDamageLocationDto
    {
        /// <summary>
        /// 縣市
        /// </summary>
        public string? County { get; set; }

        /// <summary>
        /// 危害類型類別
        /// </summary>
        public int? DamageTypeId { get; set; }

        /// <summary>
        /// 危害種類類別
        /// </summary>
        public int? DamageClassId { get; set; }

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
