using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.Entities
{
    /// <summary>
    /// 林班位置
    /// </summary>
    public class ForestCompartmentLocation : SortDefaultEntity
    {
        /// <summary>
        /// 位置
        /// </summary>
        [Required]
        [Comment("位置")]
        [StringLength(100)]
        public string Postion { get; set; } = string.Empty;

        /// <summary>
        /// 所屬管理處
        /// </summary>
        [Required]
        [Comment("所屬管理處")]
        [StringLength(100)]
        public string AffiliatedUnit { get; set; } = string.Empty;
    }
}
