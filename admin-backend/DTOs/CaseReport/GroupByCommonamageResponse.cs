namespace admin_backend.DTOs.CaseReport
{
    public class GroupByCommonamageResponse
    {
        /// <summary>
        /// 常見病蟲害ID
        /// </summary>
        public int CommonDamageId {  get; set; }

        /// <summary>
        /// 常見病蟲害
        /// </summary>
        public string CommonDamageName { get; set; } = string.Empty;

        /// <summary>
        /// 數量
        /// </summary>
        public int Count { get; set; }
    }
}
