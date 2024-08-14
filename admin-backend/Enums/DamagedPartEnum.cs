using System.ComponentModel;
using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    public enum DamagedPartEnum
    {
        [EnumMember(Value = "水泥面")]
        [Description("水泥面")]
        CementSurface = 1,

        [EnumMember(Value = "柏油面")]
        [Description("        [EnumMember(Value = \"柏油面\")]\r\n")]
        AsphaltSurface = 2,

        [EnumMember(Value = "植被泥土面 (地表有草皮或鬆潤木)")]
        [Description("植被泥土面 (地表有草皮或鬆潤木)")]
        VegetationSoilSurface = 3,

        [EnumMember(Value = "花台內")]
        [Description("花台內")]
        FlowerBed = 4,

        [EnumMember(Value = "人工鋪面 (水泥面、柏油面以外)")]
        [Description("人工鋪面 (水泥面、柏油面以外)")]
        ArtificialSurface = 5
    }
}
