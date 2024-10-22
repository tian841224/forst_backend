﻿using System.ComponentModel;
using System.Runtime.Serialization;

namespace admin_backend.Enums
{
    public enum TreePartEnum
    {
        /// <summary>
        /// 未指定
        /// </summary>
        [EnumMember(Value = "未指定")]
        [Description("未指定")]
        None = 0,

        /// <summary>
        /// 根
        /// </summary>
        [EnumMember(Value = "根")]
        [Description("根")]
        Root = 1,

        /// <summary>
        /// 侵害土壤部
        /// </summary>
        [EnumMember(Value = "侵害土壤部")]
        [Description("侵害土壤部")]
        Soil = 2,

        /// <summary>
        /// 樹幹
        /// </summary>
        [EnumMember(Value = "樹幹")]
        [Description("樹幹")] 
        Trunk = 3,

        /// <summary>
        /// 莖、枝條
        /// </summary>
        [EnumMember(Value = "莖、枝條")]
        [Description("莖、枝條")]
        StemBranches = 4,

        /// <summary>
        /// 樹枝
        /// </summary>
        [EnumMember(Value = "樹枝")]
        [Description("樹枝")]
        Branches = 5,

        /// <summary>
        /// 樹葉
        /// </summary>
        [EnumMember(Value = "樹葉")]
        [Description("樹葉")]
        Leaf = 6,

        /// <summary>
        /// 花果
        /// </summary>
        [EnumMember(Value = "花果")]
        [Description("花果")]
        FlowerFruit = 7,

        /// <summary>
        /// 全株
        /// </summary>
        [EnumMember(Value = "全株")]
        [Description("全株")]
        WholeTree = 8,

        /// <summary>
        /// 全面異常症狀病害
        /// </summary>
        [EnumMember(Value = "全面異常症狀病害")]
        [Description("全面異常症狀病害")]
        Overall = 9
    }
}
