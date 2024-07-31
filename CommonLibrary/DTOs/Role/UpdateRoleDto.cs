using CommonLibrary.DTOs.RolePermission;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.Role
{
    public class UpdateRoleDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
