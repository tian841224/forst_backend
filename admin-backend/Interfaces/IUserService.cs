using admin_backend.DTOs.AdminUser;
using admin_backend.DTOs.User;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    /// <summary>
    /// 會員
    /// </summary>
    public interface IUserService
    {
        /// <summary> 取得單筆會員 </summary>
        Task<UserResponse> Get(int Id);

        /// <summary> 取得會員 </summary>
        Task<PagedResult<UserResponse>> Get(GetUserDto dto);

        /// <summary> 新增會員 </summary>
        Task<UserResponse> Add(AddUserDto dto);
       
        /// <summary> 更新會員 </summary>
        Task<UserResponse> Update(int Id, UpdateUserDto dto);

        /// <summary> 重設會員密碼 </summary>
        Task ResetPassword(ResetPasswordDto dto);
    }
}
