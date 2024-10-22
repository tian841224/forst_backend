using System.ComponentModel;
using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    public enum AnnouncementEnum
    {
        /// <summary>
        /// 未指定
        /// </summary>
        [EnumMember(Value = "未指定")]
        [Description("未指定")]
        None = 0,

        /// <summary>
        /// 一般公告
        /// </summary>
        [EnumMember(Value = "一般公告")]
        [Description("一般公告")]
        Normal = 1,

        /// <summary>
        /// 重要公告
        /// </summary>
        [EnumMember(Value = "重要公告")]
        [Description("重要公告")]
        Important = 2,

        /// <summary>
        /// 活動公告
        /// </summary>
        [EnumMember(Value = "活動公告")]
        [Description("活動公告")]
        Events = 3,

        /// <summary>
        /// 跑馬燈
        /// </summary>
        [EnumMember(Value = "跑馬燈")]
        [Description("跑馬燈")]
        Marquee = 4,
    }
}
