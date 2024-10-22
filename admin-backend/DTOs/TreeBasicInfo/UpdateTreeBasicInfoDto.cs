namespace admin_backend.DTOs.TreeBasicInfo
{
    public class UpdateTreeBasicInfoDto
    {
        /// <summary>
        /// 學名
        /// </summary>
        public string? ScientificName { get; set; } = string.Empty;

        /// <summary>
        /// 名稱
        /// </summary>
        public string? Name { get; set; } = string.Empty;
    }
}
