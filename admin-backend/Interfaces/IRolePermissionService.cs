using admin_backend.DTOs.RolePermission;
using CommonLibrary.DTOs;

namespace admin_backend.Interfaces
{
    public interface IRolePermissionService
    {
        Task<RolePermissionResponse> Get(int Id, PagedOperationDto? dto = null);

        Task<RolePermissionResponse> Update(UpdateRolePermissionDto dto);

        Task<List<RolePermissionResponse>> Update(UpdateRolePermissionRequestDto dto);
    }
}
