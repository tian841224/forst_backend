using CommonLibrary.DTOs.Role;

namespace admin_backend.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleResponse>> Get(GetRoleDto dto);
        Task<RoleResponse> Add(AddRoleDto dto);
        Task<RoleResponse> Update(int Id, UpdateRoleDto dto);
        Task<RoleResponse> Delete(int Id);
    }
}
