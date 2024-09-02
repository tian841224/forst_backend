using Microsoft.EntityFrameworkCore;

namespace admin_backend.Entities
{
    public class SortDefaultEntity : DefaultEntity
    {
        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; } = 0;
    }
}
