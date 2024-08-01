using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.Common
{
    public class BasicDto
    {
        [Required]
        public int Id { get; set; }
    }
}
