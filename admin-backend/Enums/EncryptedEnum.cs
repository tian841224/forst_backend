using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    /// <summary>
    /// 加密方式
    /// </summary>
    public enum EncryptedEnum
    {
        /// <summary>
        /// 未設定
        /// </summary>
        [EnumMember(Value = "未指定")]
        None = 0,
        [EnumMember(Value = "SSL")]
        SSL = 1,
        [EnumMember(Value = "TSL")]
        TSL = 2,
    }
}
