using CommonLibrary.DTOs.Common;
using CommonLibrary.DTOs.DamageClass;
using CommonLibrary.DTOs.DamageType;

namespace admin_backend.Interfaces
{
    public interface IDamageClassService
    {
        Task<List<DamageClassResponse>> Get(int? Id = null, PagedOperationDto? dto = null);
        Task<List<DamageClassResponse>> Get(GetDamageClassDto dto);
        Task<DamageClassResponse> Add(AddDamageClassDto dto);
        Task<DamageClassResponse> Update(UpdateDamageClassDto dto);
        Task<DamageClassResponse> Delete(int Id);
    }
}
