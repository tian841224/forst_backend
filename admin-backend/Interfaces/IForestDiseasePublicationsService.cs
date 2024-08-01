using CommonLibrary.DTOs.Common;
using CommonLibrary.DTOs.ForestDiseasePublications;

namespace admin_backend.Interfaces
{
    public interface IForestDiseasePublicationsService
    {
        Task<List<ForestDiseasePublicationsResponse>> Get(int? Id = null);
        Task<List<ForestDiseasePublicationsResponse>> Get(GetForestDiseasePublicationsDto dto);
        Task<ForestDiseasePublicationsResponse> Add(AddForestDiseasePublicationsDto dto);
        Task<ForestDiseasePublicationsResponse> Update(int Id, UpdateForestDiseasePublicationsDto dto);
        Task<List<ForestDiseasePublicationsResponse>> UpdateSort(List<SortBasicDto> dto);
        Task<ForestDiseasePublicationsResponse> Delete(int Id);
    }
}
