using admin_backend.DTOs.TreeBasicInfo;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface ITreeBasicInfoService
    {
        Task<PagedResult<TreeBasicInfoResponse>> Get(int? id = null, PagedOperationDto? dto = null);
        Task<PagedResult<TreeBasicInfoResponse>> Get(GetTreeBasicInfoDto dto);
        Task<TreeBasicInfoResponse> Add(AddTreeBasicInfoDto dto);
        Task<TreeBasicInfoResponse> Update(int Id, UpdateTreeBasicInfoDto dto);
        Task<List<TreeBasicInfoResponse>> UpdateSort(List<SortBasicDto> dto);
        Task<TreeBasicInfoResponse> Delete(int Id);
    }
}
