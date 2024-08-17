using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    public enum TreeBaseConditionEnum
    {
        /// <summary>
        /// 水泥面
        /// </summary>
        [EnumMember(Value = "水泥面")]
        CementSurface = 1,

        /// <summary>
        /// 柏油面
        /// </summary>
        [EnumMember(Value = "柏油面")]
        AsphaltSurface = 2,

        /// <summary>
        /// 植被泥土面 (地表有草皮或鬆潤木)
        /// </summary>
        [EnumMember(Value = "植被泥土面 (地表有草皮或鬆潤木)")]
        VegetationSoilSurface = 3,

        /// <summary>
        /// 花台內
        /// </summary>
        [EnumMember(Value = "花台內")]
        FlowerBed = 4,

        /// <summary>
        /// 人工鋪面 (水泥面、柏油面以外)
        /// </summary>
        [EnumMember(Value = "人工鋪面 (水泥面、柏油面以外)")]
        ArtificialSurface = 5
    }
}
