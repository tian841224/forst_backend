using CommonLibrary.DTOs.Common;
using CommonLibrary.DTOs.TreeBasicInfo;

namespace admin_backend.Interfaces
{
    public interface ITreeBasicInfoService
    {
        Task<List<TreeBasicInfoResponse>> Get(int? id = null);
        Task<List<TreeBasicInfoResponse>> Get(GetTreeBasicInfoDto dto);
        Task<TreeBasicInfoResponse> Add(AddTreeBasicInfoDto dto);
        Task<TreeBasicInfoResponse> Update(int Id, UpdateTreeBasicInfoDto dto);
        Task<List<TreeBasicInfoResponse>> UpdateSort(List<SortBasicDto> dto);
        Task<TreeBasicInfoResponse> Delete(int Id);
    }
}
