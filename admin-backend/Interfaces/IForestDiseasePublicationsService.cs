using admin_backend.DTOs.ForestDiseasePublications;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface IForestDiseasePublicationsService
    {
        Task<PagedResult<ForestDiseasePublicationsResponse>> Get(int? Id = null, PagedOperationDto? dto = null);
        Task<PagedResult<ForestDiseasePublicationsResponse>> Get(GetForestDiseasePublicationsDto dto);
        Task<ForestDiseasePublicationsResponse> Add(AddForestDiseasePublicationsDto dto);
        Task<ForestDiseasePublicationsResponse> Update(int Id, UpdateForestDiseasePublicationsDto dto);
        Task UpdateFile(int Id, List<IFormFile> files);
        Task<List<ForestDiseasePublicationsResponse>> UpdateSort(List<SortBasicDto> dto);
        Task<ForestDiseasePublicationsResponse> Delete(int Id);
        Task DeleteFile(int Id, string fileName);
    }
}
