using admin_backend.DTOs.DamageType;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface IDamageTypeService
    {
        Task<PagedResult<DamageTypeResponse>> Get(int? Id = null, PagedOperationDto? dto = null);
        Task<PagedResult<DamageTypeResponse>> Get(GetDamageTypeDto dto);
        Task<DamageTypeResponse> Add(AddDamageTypeDto dto);
        Task<DamageTypeResponse> Update(int Id, UpdateDamageTypeDto dto);
        Task<List<DamageTypeResponse>> UpdateSort(List<SortBasicDto> dto);
        Task<DamageTypeResponse> Delete(int Id);
    }
}
