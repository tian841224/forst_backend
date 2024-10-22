using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    /// <summary>
    /// 權限
    /// </summary>
    public enum PermissionEnum
    {
        /// <summary>
        /// 未設定
        /// </summary>
        [EnumMember(Value = "未設定")]
        None = 0,

        /// <summary>
        /// 檢視
        /// </summary>
        [EnumMember(Value = "檢視")]
        View = 1,

        /// <summary>
        /// 新增
        /// </summary>
        [EnumMember(Value = "新增")]
        Add = 2,

        /// <summary>
        /// 指派
        /// </summary>
        [EnumMember(Value = "指派")]
        Sign = 3,

        /// <summary>
        /// 編輯
        /// </summary>
        [EnumMember(Value = "編輯")]
        Edit = 4,

        /// <summary>
        /// 刪除
        /// </summary>
        [EnumMember(Value = "刪除")]
        Delete = 5,
    }
}
