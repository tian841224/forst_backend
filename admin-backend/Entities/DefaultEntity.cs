using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace admin_backend.Entities
{
    public class DefaultEntity
    {
        [Key]
        public int Id { get;  set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [Comment("建立日期")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateTime { get;  set; } 

        /// <summary>
        /// 更新時間
        /// </summary>
        [Comment("更新時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get;  set; }
    }
}
