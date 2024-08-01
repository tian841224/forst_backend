using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Entities
{
    /// <summary>
    /// 樹木基本資料
    /// </summary>
    public class TreeBasicInfo : SortDefaultEntity
    {
        /// <summary>
        /// 學名
        /// </summary>
        [Required]
        [Comment("學名")]
        [StringLength(100)]
        public string ScientificName { get; set; } = string.Empty;
       
        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        [Comment("名稱")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
