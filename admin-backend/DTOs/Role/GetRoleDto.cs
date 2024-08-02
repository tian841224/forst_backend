using CommonLibrary.DTOs;

namespace admin_backend.DTOs.Role
{
    public class GetRoleDto : PagedOperationDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }
    }
}
