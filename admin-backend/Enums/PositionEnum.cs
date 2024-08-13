using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    /// <summary>
    /// 廣告位置
    /// </summary>
    public enum PositionEnum
    {
        /// <summary>
        /// 未設定
        /// </summary>
        [EnumMember(Value = "未設定")]
        None = 0,

        /// <summary>
        /// 橫幅
        /// </summary>
        [EnumMember(Value = "橫幅")]
        Banner = 1,

        /// <summary>
        /// 首頁
        /// </summary>
        [EnumMember(Value = "首頁")]
        HomePage = 2,
    }
}
