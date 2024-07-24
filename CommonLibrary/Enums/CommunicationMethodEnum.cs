using System.ComponentModel;

namespace CommonLibrary.Enums
{
    public enum CommunicationMethodEnum
    {
        /// <summary>
        /// 線上
        /// </summary>
        [Description("線上")]
        Online = 1,

        /// <summary>
        /// e-mail
        /// </summary>
        [Description("e-mail")]
        Email = 2,

        /// <summary>
        /// 自送
        /// </summary>
        [Description("自送")]
        SelfDelivery = 3,

        /// <summary>
        /// 郵寄
        /// </summary>
        [Description("郵寄")]
        Mail = 4,

        /// <summary>
        /// 電話諮詢
        /// </summary>
        [Description("電話諮詢")]
        PhoneConsultation = 5
    }
}
