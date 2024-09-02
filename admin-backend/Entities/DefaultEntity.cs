using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace admin_backend.Entities
{
    public class DefaultEntity
    {
        private static readonly TimeZoneInfo TaipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");

        [Key]
        public int Id { get;  set; }

        private DateTime _createTime;
        [Comment("建立日期")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateTime
        {
            get => TimeZoneInfo.ConvertTimeFromUtc(_createTime, TaipeiTimeZone);
            set => _createTime = TimeZoneInfo.ConvertTimeToUtc(value, TaipeiTimeZone);
        }

        private DateTime _updateTime;
        [Comment("更新時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime
        {
            get => TimeZoneInfo.ConvertTimeFromUtc(_updateTime, TaipeiTimeZone);
            set => _updateTime = TimeZoneInfo.ConvertTimeToUtc(value, TaipeiTimeZone);
        }
    }
}
