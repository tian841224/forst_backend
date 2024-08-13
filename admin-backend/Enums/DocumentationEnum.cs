using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    /// <summary>
    /// 使用條款類型
    /// </summary>
    public enum DocumentationEnum
    {
        /// <summary>
        /// 未設定
        /// </summary>
        [EnumMember(Value = "未設定")]
        None = 0,

        /// <summary>
        /// 同意書
        /// </summary>
        [EnumMember(Value = "同意書")]
        ConsentForm = 1,

        /// <summary>
        /// 使用說明
        /// </summary>
        [EnumMember(Value = "使用說明")]
        UserGuide = 2,
    }
}
