using admin_backend.DTOs.CommonDamage;
using admin_backend.DTOs.DamageType;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface ICommonDamageService
    {
        Task<PagedResult<CommonDamageResponse>> Get(int? Id = null, PagedOperationDto? dto = null);
        Task<PagedResult<CommonDamageResponse>> Get(GetCommonDamageDto dto);
        Task<CommonDamageResponse> Add(AddCommonDamageDto dto);
        Task<CommonDamageResponse> Update(int Id, UpdateCommonDamageDto dto);
        Task<List<CommonDamageResponse>> UpdateSort(List<SortBasicDto> dto);
        Task<List<CommonDamagePhotoResponse>> UploadFile(int Id, List<CommonDamagePhotoDto> dto);
        Task<CommonDamageResponse> Delete(int Id);
        Task DeleteFile(int Id, string fileId);
    }
}
