using System.ComponentModel;
using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    /// <summary>
    /// 站台
    /// </summary>
    public enum WebsiteEnum
    {

        /// <summary>
        /// 未設定
        /// </summary>
        [EnumMember(Value = "未設定")]
        [Description("未設定")]
        None = 0,

        /// <summary>
        /// 林業自然保育署
        /// </summary>
        [EnumMember(Value = "林業自然保育署")]
        [Description("林業自然保育署")]
        Forest = 1,

        /// <summary>
        /// 林業試驗所
        /// </summary>
        [EnumMember(Value = "林業試驗所")]
        [Description("林業試驗所")]
        TFRI = 2,
    }
}
