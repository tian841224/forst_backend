using System.ComponentModel;
using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    public enum CaseStatusEnum
    {
        /// <summary>
        /// 未設定
        /// </summary>
        [EnumMember(Value = "未設定")]
        [Description("未設定")]
        None = 0,

        /// <summary>
        /// 暫存
        /// </summary>
        [EnumMember(Value = "暫存")]
        [Description("暫存")]
        Temp = 1,

        /// <summary>
        /// 待指派
        /// </summary>
        [EnumMember(Value = "待指派")]
        [Description("待指派")]
        Pending_Assignment = 2,

        /// <summary>
        /// 待簽核
        /// </summary>
        [EnumMember(Value = "待簽核")]
        [Description("待簽核")]
        Pending_Review = 3,

        /// <summary>
        /// 已結案
        /// </summary>
        [EnumMember(Value = "已結案")]
        [Description("已結案")]
        Completed = 4,

        /// <summary>
        /// 已刪除
        /// </summary>
        [EnumMember(Value = "已刪除")]
        [Description("已刪除")]
        Delete = 5,

        /// <summary>
        /// 退回
        /// </summary>
        [EnumMember(Value = "退回")]
        [Description("退回")]
        Return = 6,

        ///// <summary>
        ///// 不限
        ///// </summary>
        //[EnumMember(Value = "全部")]
        //[Description("全部")]
        //All = 99,
    }
}
