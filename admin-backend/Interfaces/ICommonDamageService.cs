using admin_backend.DTOs.CommonDamage;
using admin_backend.DTOs.DamageType;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    /// <summary>
    /// 常見病蟲害
    /// </summary>
    public interface ICommonDamageService
    {
        /// <summary>取得常見病蟲害</summary>
        Task<PagedResult<CommonDamageResponse>> Get(int? Id = null, PagedOperationDto? dto = null);
        /// <summary>取得常見病蟲害</summary>
        Task<PagedResult<CommonDamageResponse>> Get(GetCommonDamageDto dto);
        /// <summary> 新增常見病蟲害 </summary>
        Task<CommonDamageResponse> Add(AddCommonDamageDto dto);
        /// <summary>修改常見病蟲害</summary>
        Task<CommonDamageResponse> Update(int Id, UpdateCommonDamageDto dto);
        /// <summary> 上傳常見病蟲害圖片 </summary>
        Task<List<CommonDamageResponse>> UpdateSort(List<SortBasicDto> dto);
        /// <summary> 修改常見病蟲害排序 </summary>
        Task<List<CommonDamagePhotoResponse>> UpdateFileSort(int Id, List<UpdateFileSortDto> dto);
        /// <summary> 上傳常見病蟲害圖片 </summary>
        Task<List<CommonDamagePhotoResponse>> UploadFile(int Id, CommonDamagePhotoDto dto);
        /// <summary> 刪除常見病蟲害 </summary>
        Task<CommonDamageResponse> Delete(int Id);
        /// <summary> 刪除圖片 </summary>
        Task DeleteFile(int Id, int fileId);
    }
}
