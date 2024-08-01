using CommonLibrary.DTOs.Login;
using CommonLibrary.DTOs.Common;

namespace admin_backend.Interfaces
{
    public interface ILoginServices
    {
        Task<IdentityResultDto> Login(LoginDto dto);
        Task ResetPassword(ResetPasswordDto dto);
    }
}
