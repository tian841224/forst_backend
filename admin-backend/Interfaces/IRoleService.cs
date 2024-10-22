using admin_backend.DTOs.Role;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface IRoleService
    {
        Task<PagedResult<RoleResponse>> Get(GetRoleDto dto);
        Task<RoleResponse> Add(AddRoleDto dto);
        Task<RoleResponse> Update(int Id, UpdateRoleDto dto);
        Task<RoleResponse> Delete(int Id);
    }
}
