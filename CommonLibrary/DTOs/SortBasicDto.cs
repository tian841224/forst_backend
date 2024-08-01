using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs
{
    public class SortBasicDto
    {
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int? Sort { get; set; } = 0;
    }
}
