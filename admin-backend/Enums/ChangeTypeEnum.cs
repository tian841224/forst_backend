using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    /// <summary>
    /// 異動類型
    /// </summary>
    public enum ChangeTypeEnum
    {
        /// <summary>
        /// 未設定
        /// </summary>
        [EnumMember(Value = "未指定")]
        None = 0,

        /// <summary>
        /// 新增
        /// </summary>
        [EnumMember(Value = "新增")]
        Add = 1,

        /// <summary>
        /// 指派
        /// </summary>
        [EnumMember(Value = "指派")]
        Sign = 2,

        /// <summary>
        /// 編輯
        /// </summary>
        [EnumMember(Value = "編輯")]
        Edit = 3,

        /// <summary>
        /// 刪除
        /// </summary>
        [EnumMember(Value = "刪除")]
        Delete = 4,
    }
}
