using admin_backend.DTOs.User;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserResponse>> Get(GetUserDto dto);
        Task<UserResponse> Add(AddUserDto dto);
        Task<UserResponse> Update(int Id, UpdateUserDto dto);
    }
}
