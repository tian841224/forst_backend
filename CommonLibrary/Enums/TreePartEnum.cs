using System.ComponentModel;

namespace CommonLibrary.Enums
{
    public enum TreePartEnum
    {
        /// <summary>
        /// 未指定
        /// </summary>
        [Description("未指定")]
        None = 0,

        /// <summary>
        /// 根
        /// </summary>
        [Description("根")]
        Root = 1,

        /// <summary>
        /// 侵害土壤部
        /// </summary>
        [Description("侵害土壤部")]
        Soil = 2,

        /// <summary>
        /// 樹幹
        /// </summary>
        [Description("樹幹")]
        Trunk = 3,

        /// <summary>
        /// 莖、枝條
        /// </summary>
        [Description("莖、枝條")]
        StemBranches = 4,

        /// <summary>
        /// 樹枝
        /// </summary>
        [Description("樹枝")]
        Branches = 5,

        /// <summary>
        /// 樹葉
        /// </summary>
        [Description("樹葉")]
        Leaf = 6,

        /// <summary>
        /// 花果
        /// </summary>
        [Description("花果")]
        FlowerFruit = 7,

        /// <summary>
        /// 全株
        /// </summary>
        [Description("全株")]
        WholeTree = 8,

        /// <summary>
        /// 全面異常症狀病害
        /// </summary>
        [Description("全面異常症狀病害")]
        Overall = 9
    }
}
