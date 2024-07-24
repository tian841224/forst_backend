using System.ComponentModel;

namespace CommonLibrary.Enums
{
    public enum LocationTypeEnum
    {
        [Description("公園、校園")]
        ParkCampus = 1,

        [Description("人行道")]
        Sidewalk = 2,

        [Description("花台內")]
        FlowerBed = 3,

        [Description("建築周邊")]
        BuildingSurroundings = 4,

        [Description("林地")]
        Forest = 5,

        [Description("苗圃")]
        Nursery = 6,

        [Description("農地")]
        Farmland = 7,

        [Description("空地")]
        VacantLand = 8
    }
}
