using admin_backend.DTOs.User;

namespace admin_backend.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponse>> Get(GetUserDto dto);
        Task<UserResponse> Add(AddUserDto dto);
        Task<UserResponse> Update(int Id, UpdateUserDto dto);

    }
}
