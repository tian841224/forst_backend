using CommonLibrary.DTOs.Common;
using CommonLibrary.DTOs.RolePermission;

namespace admin_backend.Interfaces
{
    public interface IRolePermissionService
    {
        Task<RolePermissionResponse> Get(int Id, PagedOperationDto? dto = null);

        Task<RolePermissionResponse> Update(UpdateRolePermissionDto dto);

        Task<List<RolePermissionResponse>> Update(UpdateRolePermissionRequestDto dto);
    }
}
