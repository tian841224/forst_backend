using System.ComponentModel;

namespace admin_backend.Enums
{
    public enum TreeBaseConditionEnum
    {
        [Description("水泥面")]
        CementSurface,

        [Description("柏油面")]
        AsphaltSurface,

        [Description("植被泥土面 (地表有草皮或鬆潤木)")]
        VegetationSoilSurface,

        [Description("花台內")]
        FlowerBed,

        [Description("人工鋪面 (水泥面、柏油面以外)")]
        ArtificialSurface
    }
}
