using admin_backend.DTOs.AdminUser;
using admin_backend.DTOs.Login;
using CommonLibrary.DTOs;

namespace admin_backend.Interfaces
{
    /// <summary>
    /// 登入相關
    /// </summary>
    public interface ILoginServices
    {
        /// <summary> 登入後台 </summary>
        Task<IdentityResultDto> LoginBackEnd(LoginDto dto);

        /// <summary> 登入前台 </summary>
        Task<IdentityResultDto> LoginFrontEnd(LoginFrontEndDto dto);
    }
}
