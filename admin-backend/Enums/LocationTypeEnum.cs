using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    public enum LocationTypeEnum
    {
        /// <summary>
        /// 公園、校園
        /// </summary>
        [EnumMember(Value = "公園、校園")]
        ParkCampus = 1,

        /// <summary>
        /// 人行道
        /// </summary>
        [EnumMember(Value = "人行道")]
        Sidewalk = 2,

        /// <summary>
        /// 花台內
        /// </summary>
        [EnumMember(Value = "花台內")]
        FlowerBed = 3,

        /// <summary>
        /// 建築周邊
        /// </summary>
        [EnumMember(Value = "建築周邊")]
        BuildingSurroundings = 4,

        /// <summary>
        /// 林地
        /// </summary>
        [EnumMember(Value = "林地")]
        Forest = 5,

        /// <summary>
        /// 苗圃
        /// </summary>
        [EnumMember(Value = "苗圃")]
        Nursery = 6,

        /// <summary>
        /// 農地
        /// </summary>
        [EnumMember(Value = "農地")]
        Farmland = 7,

        /// <summary>
        /// 空地
        /// </summary>
        [EnumMember(Value = "空地")]
        VacantLand = 8
    }
}
