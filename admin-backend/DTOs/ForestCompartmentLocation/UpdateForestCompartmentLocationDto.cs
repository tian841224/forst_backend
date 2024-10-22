﻿namespace admin_backend.DTOs.ForestCompartmentLocation
{
    public class UpdateForestCompartmentLocationDto
    {
        /// <summary>
        /// 位置
        /// </summary>
        public string? Position { get; set; } = string.Empty;

        /// <summary>
        /// 所屬管理處
        /// </summary>
        public string? AffiliatedUnit { get; set; } = string.Empty;
    }
}
