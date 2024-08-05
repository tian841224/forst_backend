using admin_backend.DTOs.AdminUser;
using admin_backend.Entities;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface IAdminUserServices
    {
         Task<AdminUserResponse> Get();

        Task<PagedResult<AdminUserResponse>> Get(GetAdminUserDto dto);
        Task<AdminUser> Add(AddAdminUserDto dto);

        Task<AdminUser> Update(int Id, UpdateAdminUserDto dto);
    }
}
