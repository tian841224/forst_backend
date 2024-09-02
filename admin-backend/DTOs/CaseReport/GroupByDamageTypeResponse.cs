namespace admin_backend.DTOs.CaseReport
{
    public class GroupByDamageTypeResponse
    {
        /// <summary>
        /// 危害種類類別ID
        /// </summary>
        public int DamageTypeId { get; set; }

        /// <summary>
        /// 危害種類類別
        /// </summary>
        public string DamageTypeName { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Count { get; set; }
    }
}
