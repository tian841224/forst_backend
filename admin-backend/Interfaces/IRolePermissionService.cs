using admin_backend.DTOs.RolePermission;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface IRolePermissionService
    {
        Task<RolePermissionResponse> Get(int Id);

        Task<PagedResult<RolePermissionResponse>> Get(PagedOperationDto? dto = null);

        Task<RolePermissionResponse> Update(UpdateRolePermissionDto dto);

        Task<List<RolePermissionResponse>> Update(UpdateRolePermissionRequestDto dto);
    }
}
