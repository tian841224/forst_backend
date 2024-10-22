namespace admin_backend.DTOs.CaseReport
{
    public class GroupByDamageClassResponse
    {
        /// <summary>
        /// 危害種類ID
        /// </summary>
        public int DamageClassId { get; set; }

        /// <summary>
        /// 危害種類類別
        /// </summary>
        public string DamageClassName { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Count { get; set; }
    }
}
