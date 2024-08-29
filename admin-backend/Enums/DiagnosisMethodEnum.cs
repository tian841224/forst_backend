using System.ComponentModel;
using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    /// <summary>
    /// 診斷方式
    /// </summary>
    public enum DiagnosisMethodEnum
    {
        /// <summary>
        /// 未指定
        /// </summary>
        [EnumMember(Value = "未指定")]
        [Description("未指定")]
        None = 0,

        /// <summary>
        /// 現地診察
        /// </summary>
        [EnumMember(Value = "現地診察")]
        [Description("現地診察")]
        OnSiteInspection = 1,

        /// <summary>
        /// 樣本檢驗
        /// </summary>
        [EnumMember(Value = "樣本檢驗")]
        [Description("樣本檢驗")]
        SampleInspection = 2,

        /// <summary>
        /// 資料判讀
        /// </summary>
        [EnumMember(Value = "資料判讀")]
        [Description("資料判讀")]
        DataAnalysis = 3,

        /// <summary>
        /// 電話訪談
        /// </summary>
        [EnumMember(Value = "電話訪談")]
        [Description("電話訪談")]
        PhoneConsultation = 4,
    }
}
