using CommonLibrary.DTOs.Common;
using CommonLibrary.DTOs.Login;

namespace CommonLibrary.Interface
{
    public interface IIdentityService
    {
        ClaimDto GetUser();
        string GenerateToken(GenerateTokenDto dto);

    }
}
