using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.ForestCompartmentLocation
{
    public class DeleteForestCompartmentLocationDto
    {
        [Required]
        public int Id { get; set; }
    }
}
