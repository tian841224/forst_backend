using admin_backend.DTOs.RolePermission;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface IRolePermissionService
    {
        /// <summary> 取得角色權限 </summary>
        Task<RolePermissionResponse> Get(int Id);
        /// <summary> 取得角色權限 </summary>
        Task<PagedResult<RolePermissionResponse>> Get(PagedOperationDto? dto = null);
        /// <summary> 新增角色權限 </summary>
        Task<RolePermissionResponse> Add(AddRolePermissionDto dto);
        /// <summary> 修改角色權限 </summary>
        Task<RolePermissionResponse> Update(UpdateRolePermissionDto dto);
        /// <summary> 修改角色權限 </summary>
        Task<List<RolePermissionResponse>> Update(UpdateRolePermissionRequestDto dto);
    }
}
