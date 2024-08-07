using admin_backend.DTOs.AdminUser;
using admin_backend.Entities;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface IAdminUserServices
    {
        Task<AdminUserResponse> Get();
        Task<PagedResult<AdminUserResponse>> Get(GetAdminUserDto dto);
        Task<AdminUserResponse> Add(AddAdminUserDto dto);
        Task<AdminUserResponse> Update(int Id, UpdateAdminUserDto dto);
    }
}
