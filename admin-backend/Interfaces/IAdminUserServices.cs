using CommonLibrary.DTOs.AdminUser;
using CommonLibrary.Entities;

namespace admin_backend.Interfaces
{
    public interface IAdminUserServices
    {
         Task<AdminUserResponse> Get();

        Task<List<AdminUserResponse>> Get(GetAdminUserDto dto);

        Task<AdminUser> Add(AddAdminUserDto dto);

        Task<AdminUser> Update(int Id, UpdateAdminUserDto dto);
    }
}
