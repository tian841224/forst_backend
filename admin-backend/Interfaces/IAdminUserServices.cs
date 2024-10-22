using admin_backend.DTOs.AdminUser;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    /// <summary>
    /// 後台帳號
    /// </summary>
    public interface IAdminUserServices
    {
        /// <summary> 取得當前後台帳號 </summary>
        Task<AdminUserResponse> Get();

        /// <summary> 取得後台帳號 </summary>
        Task<PagedResult<AdminUserResponse>> Get(GetAdminUserDto dto);

        /// <summary> 新增後台帳號 </summary>
        Task<AdminUserResponse> Add(AddAdminUserDto dto);

        /// <summary> 修改後台帳號 </summary>
        Task<AdminUserResponse> Update(int Id, UpdateAdminUserDto dto);

        /// <summary> 重設密碼 </summary>
        Task ResetPassword(ResetPasswordDto dto);
    }
}
