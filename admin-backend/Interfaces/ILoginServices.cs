using admin_backend.DTOs.Login;
using CommonLibrary.DTOs;

namespace admin_backend.Interfaces
{
    public interface ILoginServices
    {
        Task<IdentityResultDto> Login(LoginDto dto);
        Task ResetPassword(ResetPasswordDto dto);
    }
}
