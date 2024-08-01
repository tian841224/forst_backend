using CommonLibrary.DTOs.Common;
using CommonLibrary.DTOs.ForestCompartmentLocation;

namespace admin_backend.Interfaces
{
    public interface IForestCompartmentLocationService
    {
        Task<List<ForestCompartmentLocationResponse>> Get(int? Id = null);
        Task<List<ForestCompartmentLocationResponse>> Get(GetForestCompartmentLocationDto dto);
        Task<ForestCompartmentLocationResponse> Add(AddForestCompartmentLocationDto dto);
        Task<ForestCompartmentLocationResponse> Update(int Id, UpdateForestCompartmentLocationDto dto);
        Task<List<ForestCompartmentLocationResponse>> UpdateSort(List<SortBasicDto> dto);
        Task<ForestCompartmentLocationResponse> Delete(int Id);
    }
}
