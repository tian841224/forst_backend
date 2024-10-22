using CommonLibrary.DTOs;

namespace admin_backend.DTOs.Role
{
    public class RoleResponse : DefaultResponseDto
    {
        /// <summary>
        /// 角色名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
