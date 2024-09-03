using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace admin_backend.Entities
{
    public class DefaultEntity : BaseDefaultEntity
    {
        [Comment("建立日期")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override DateTime CreateTime { get; set; } = DateTime.Now;

        [Comment("更新時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public override DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
