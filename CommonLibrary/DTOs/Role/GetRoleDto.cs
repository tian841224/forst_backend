using CommonLibrary.DTOs.Common;

namespace CommonLibrary.DTOs.Role
{
    public class GetRoleDto : PagedOperationDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }
    }
}
