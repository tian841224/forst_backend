using System.ComponentModel.DataAnnotations;
using CommonLibrary.DTOs.Common;

namespace CommonLibrary.DTOs.ForestCompartmentLocation
{
    public class AddForestCompartmentLocationDto : SortDto
    {
        /// <summary>
        /// 位置
        /// </summary>
        [Required]
        public string Postion { get; set; } = string.Empty;

        /// <summary>
        /// 所屬管理處
        /// </summary>
        [Required]
        public string AffiliatedUnit { get; set; } = string.Empty;
    }
}
