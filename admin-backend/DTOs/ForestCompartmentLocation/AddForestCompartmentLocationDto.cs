using CommonLibrary.DTOs;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.ForestCompartmentLocation
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
