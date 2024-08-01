using CommonLibrary.DTOs.Login;
using CommonLibrary.DTOs;

namespace admin_backend.Interfaces
{
    public interface ILoginServices
    {
        Task<IdentityResultDto> Login(LoginDto dto);
    }
}
