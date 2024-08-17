using admin_backend.DTOs.ForestCompartmentLocation;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    /// <summary>
    /// 林班位置
    /// </summary>
    public interface IForestCompartmentLocationService
    {
        Task<PagedResult<ForestCompartmentLocationResponse>> Get(int? Id = null, PagedOperationDto? dto = null);
        Task<PagedResult<ForestCompartmentLocationResponse>> Get(GetForestCompartmentLocationDto dto);
        Task<ForestCompartmentLocationResponse> Add(AddForestCompartmentLocationDto dto);
        Task<ForestCompartmentLocationResponse> Update(int Id, UpdateForestCompartmentLocationDto dto);
        Task<List<ForestCompartmentLocationResponse>> UpdateSort(List<SortBasicDto> dto);
        Task<ForestCompartmentLocationResponse> Delete(int Id);
    }
}
