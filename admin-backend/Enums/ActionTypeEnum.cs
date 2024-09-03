using System.ComponentModel;
using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    /// <summary>
    /// 操作類型
    /// </summary>
    public enum ActionTypeEnum
    {
        /// <summary>
        /// 未指定
        /// </summary>
        [EnumMember(Value = "未指定")]
        [Description("未指定")]
        None = 0,

        /// <summary>
        /// 新增
        /// </summary>
        [EnumMember(Value = "新增")]
        [Description("新增")]
        Add = 1,

        /// <summary>
        /// 指派
        /// </summary>
        [EnumMember(Value = "指派")]
        [Description("指派")]
        Assign = 2,

        /// <summary>
        /// 退回
        /// </summary>
        [EnumMember(Value = "退回")]
        [Description("退回")]
        Return = 3,
    }
}
