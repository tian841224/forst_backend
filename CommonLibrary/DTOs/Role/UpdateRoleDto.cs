using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.Role
{
    public class UpdateRoleDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
