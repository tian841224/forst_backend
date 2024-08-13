using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    /// <summary>
    /// 使用狀態
    /// </summary>
    public enum StatusEnum
    {
        /// <summary>
        /// 關閉
        /// </summary>
        [EnumMember(Value = "關閉")]
        Close = 0,

        /// <summary>
        /// 開啟
        /// </summary>
        [EnumMember(Value = "開啟")]
        Open = 1,

        /// <summary>
        /// 停用
        /// </summary>
        [EnumMember(Value = "停用")]
        Stop = 2,
    }
}
