using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.Role
{
    public class AddRoleDto
    {
        [Required]
        public string Name { get; set; }
    }
}
