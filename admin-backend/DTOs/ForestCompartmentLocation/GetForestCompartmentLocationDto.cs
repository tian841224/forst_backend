using CommonLibrary.DTOs;

namespace admin_backend.DTOs.ForestCompartmentLocation
{
    public class GetForestCompartmentLocationDto : PagedOperationDto
    {
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string? Keyword { get; set; } = string.Empty;
    }
}