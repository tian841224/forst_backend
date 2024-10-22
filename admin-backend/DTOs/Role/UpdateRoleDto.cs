using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.Role
{
    public class UpdateRoleDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
