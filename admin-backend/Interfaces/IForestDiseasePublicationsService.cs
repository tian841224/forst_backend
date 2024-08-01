using CommonLibrary.DTOs.ForestDiseasePublications;

namespace admin_backend.Interfaces
{
    public interface IForestDiseasePublicationsService
    {
        Task<List<ForestDiseasePublicationsResponse>> Get(int? Id = null);
        Task<ForestDiseasePublicationsResponse> Add(AddForestDiseasePublicationsDto dto);
        Task<ForestDiseasePublicationsResponse> Update(int Id, UpdateForestDiseasePublicationsDto dto);
        Task<ForestDiseasePublicationsResponse> Delete(int Id);
    }
}
