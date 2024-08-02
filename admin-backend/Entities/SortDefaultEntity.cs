using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace admin_backend.Entities
{
    public class SortDefaultEntity
    {
        [Key]
        public int Id { get; protected set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [Required]
        [Comment("建立日期")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateTime { get; protected set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        [Comment("更新時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; protected set; } = DateTime.UtcNow;

        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; } = 0;
    }
}
