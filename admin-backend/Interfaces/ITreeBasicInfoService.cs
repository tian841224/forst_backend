using CommonLibrary.DTOs.TreeBasicInfo;

namespace admin_backend.Interfaces
{
    public interface ITreeBasicInfoService
    {
        Task<List<TreeBasicInfoResponse>> Get(int? id = null);
        Task<TreeBasicInfoResponse> Add(AddTreeBasicInfoDto dto);
        Task<TreeBasicInfoResponse> Update(int Id, UpdateTreeBasicInfoDto dto);
        Task<TreeBasicInfoResponse> Delete(int Id);
    }
}
