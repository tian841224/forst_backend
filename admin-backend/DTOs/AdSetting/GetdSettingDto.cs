using CommonLibrary.DTOs;

namespace admin_backend.DTOs.AdSetting
{
    public class GetdSettingDto
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; } = new PagedOperationDto();
    }
}
