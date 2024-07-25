using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.Role
{
    public class DeleteRoleDto
    {
        [Required]
        public int Id { get; set; }
    }
}
