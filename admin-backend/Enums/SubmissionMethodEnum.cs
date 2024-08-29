using System.ComponentModel;
using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    /// <summary>
    /// 送件方式
    /// </summary>
    public enum SubmissionMethodEnum
    {
        /// <summary>
        /// 未設定
        /// </summary>
        [EnumMember(Value = "未設定")]
        [Description("未設定")]
        None = 0,

        /// <summary>
        /// 線上
        /// </summary>
        [EnumMember(Value = "線上")]
        [Description("線上")]
        Online = 1,

        /// <summary>
        /// Email
        /// </summary>
        [EnumMember(Value = "Email")]
        [Description("Email")]
        Email = 2,

        /// <summary>
        /// 自送
        /// </summary>
        [EnumMember(Value = "自送")]
        [Description("自送")]
        SelfDelivery = 3,

        /// <summary>
        /// 郵寄
        /// </summary>
        [EnumMember(Value = "郵寄")]
        [Description("郵寄")]
        Postal = 4,

        /// <summary>
        /// 電話諮詢
        /// </summary>
        [EnumMember(Value = "電話諮詢")]
        [Description("電話諮詢")]
        PhoneInquiry = 5 
    }
}
