using System.ComponentModel;
using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    public enum DamagedPartEnum
    {
        [EnumMember(Value = "葉")]
        [Description("葉")]
        Leaf = 1,

        [EnumMember(Value = "花")]
        [Description("花")]
        Flower = 2,

        [EnumMember(Value = "枝")]
        [Description("枝")]
        Branch = 3,

        [EnumMember(Value = "幹")]
        [Description("幹")]
        Trunk = 4,

        [EnumMember(Value = "葉部")]
        [Description("葉部")]
        LeafPart = 5,

        [EnumMember(Value = "侵害土壤部")]
        [Description("侵害土壤部")]
        SoilDamage = 6,

        [EnumMember(Value = "全面異常症狀病害")]
        [Description("全面異常症狀病害")]
        OverallAbnormalDisease = 7
    }
}
