using CommonLibrary.DTOs;

namespace admin_backend.DTOs.ForestCompartmentLocation
{
    public class GetForestCompartmentLocationDto
    {
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string? Keyword { get; set; } = string.Empty;

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; }
    }
}