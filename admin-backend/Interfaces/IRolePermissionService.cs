using CommonLibrary.DTOs.RolePermission;

namespace admin_backend.Interfaces
{
    public interface IRolePermissionService
    {
        Task<RolePermissionResponse> Get(int Id);

        Task<RolePermissionResponse> Update(UpdateRolePermissionDto dto);

        Task<List<RolePermissionResponse>> Update(UpdateRolePermissionRequestDto dto);
    }
}
