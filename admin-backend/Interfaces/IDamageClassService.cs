using admin_backend.DTOs.DamageClass;
using admin_backend.DTOs.DamageType;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface IDamageClassService
    {
        Task<PagedResult<DamageClassResponse>> Get(int? Id = null, PagedOperationDto? dto = null);
        Task<PagedResult<DamageClassResponse>> Get(GetDamageClassDto dto);
        Task<DamageClassResponse> Add(AddDamageClassDto dto);
        Task<DamageClassResponse> Update(int Id, UpdateDamageClassDto dto);
        Task<DamageClassResponse> Delete(int Id);
    }
}
