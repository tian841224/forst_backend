using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibrary.Entities
{
    public class DefaultEntity
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
    }
}
