using CommonLibrary.DTOs;

namespace admin_backend.DTOs.TreeBasicInfo
{
    public class GetTreeBasicInfoDto : PagedOperationDto
    {
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string? Keyword { get; set; } = string.Empty;
    }
}
