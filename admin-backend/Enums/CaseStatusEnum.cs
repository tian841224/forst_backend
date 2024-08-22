using System.ComponentModel;
using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    public enum CaseStatusEnum
    {
        /// <summary>
        /// 不限
        /// </summary>
        [EnumMember(Value = "不限")]
        [Description("不限")]
        All = 0,

        /// <summary>
        /// 待指派
        /// </summary>
        [EnumMember(Value = "待指派")]
        [Description("待指派")]
        Pending_Assignment = 1,

        /// <summary>
        /// 待簽核
        /// </summary>
        [EnumMember(Value = "待簽核")]
        [Description("待簽核")]
        Pending_Review = 2,

        /// <summary>
        /// 已結案
        /// </summary>
        [EnumMember(Value = "已結案")]
        [Description("已結案")]
        Completed = 3,

        /// <summary>
        /// 已刪除
        /// </summary>
        [EnumMember(Value = "已刪除")]
        [Description("已刪除")]
        DELETED = 4,
    }
}
