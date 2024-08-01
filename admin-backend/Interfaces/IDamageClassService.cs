using CommonLibrary.DTOs.DamageClass;

namespace admin_backend.Interfaces
{
    public interface IDamageClassService
    {
        Task<List<DamageClassResponse>> Get(int? Id = null);
        Task<List<DamageClassResponse>> Get(int TypeId);
        Task<DamageClassResponse> Add(AddDamageClassDto dto);
        Task<DamageClassResponse> Update(UpdateDamageClassDto dto);
        Task<DamageClassResponse> Delete(int Id);
    }
}
