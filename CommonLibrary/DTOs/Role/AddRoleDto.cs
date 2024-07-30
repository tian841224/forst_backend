using CommonLibrary.DTOs.RolePermission;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.Role
{
    public class AddRoleDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public  List<AddRolePermissionDto> RolePermission { get; set; }
    }
}
