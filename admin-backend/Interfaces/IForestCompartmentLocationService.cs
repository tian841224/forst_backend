using CommonLibrary.DTOs.ForestCompartmentLocation;

namespace admin_backend.Interfaces
{
    public interface IForestCompartmentLocationService
    {
        Task<List<ForestCompartmentLocationResponse>> Get(int? Id = null);
        Task<ForestCompartmentLocationResponse> Add(AddForestCompartmentLocationDto dto);
        Task<ForestCompartmentLocationResponse> Update(int Id, UpdateForestCompartmentLocationDto dto);
        Task<ForestCompartmentLocationResponse> Delete(int Id);
    }
}
