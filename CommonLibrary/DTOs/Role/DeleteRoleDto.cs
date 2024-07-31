using CommonLibrary.DTOs.RolePermission;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.Role
{
    public class DeleteRoleDto
    {
        [Required]
        public int Id { get; set; }

        public List<DeleteRolePermissionDto> RolePermission { get; set; } = null!;
    }
}
