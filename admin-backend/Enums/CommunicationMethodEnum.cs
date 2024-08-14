using System.ComponentModel;
using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    public enum CommunicationMethodEnum
    {
        /// <summary>
        /// 線上
        /// </summary>
        [EnumMember(Value = "線上")]
        [Description("線上")]
        Online = 1,

        /// <summary>
        /// e-mail
        /// </summary>
        [EnumMember(Value = "e-mail")]
        [Description("e-mail")]
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
        Mail = 4,

        /// <summary>
        /// 電話諮詢
        /// </summary>
        [EnumMember(Value = "電話諮詢")]
        [Description("電話諮詢")]
        PhoneConsultation = 5
    }
}
