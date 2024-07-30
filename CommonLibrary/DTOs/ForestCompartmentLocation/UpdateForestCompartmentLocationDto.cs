using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.ForestCompartmentLocation
{
    public class UpdateForestCompartmentLocationDto
    {
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string? Postion { get; set; } = string.Empty;

        /// <summary>
        /// 所屬管理處
        /// </summary>
        public string? AffiliatedUnit { get; set; } = string.Empty;
    }
}
