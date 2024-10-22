using admin_backend.DTOs.AdSetting;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    /// <summary>
    /// 官網廣告版位
    /// </summary>
    public interface IAdSettingService
    {
        /// <summary>取得官網廣告版位</summary>
        Task<PagedResult<AdSettingResponse>> Get(int? Id = null, PagedOperationDto? dto = null);

        /// <summary>取得官網廣告版位</summary>
        Task<PagedResult<AdSettingResponse>> Get(GetdSettingDto dto);

        /// <summary> 新增官網廣告版位 </summary>
        Task<AdSettingResponse> Add(AddAdSettingDto dto);

        /// <summary>修改官網廣告版位</summary>
        Task<AdSettingResponse> Update(int Id, UpdateAdSettingDto dto);

        /// <summary> 上傳官網廣告版位圖片 </summary>
        Task UploadFile(int Id, AdSettingPhotoDto dto);

        /// <summary> 刪除官網廣告版位 </summary>
        Task<AdSettingResponse> Delete(int Id);
    }
}
