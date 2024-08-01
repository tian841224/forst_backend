namespace CommonLibrary.DTOs.ForestCompartmentLocation
{
    public class ForestCompartmentLocationResponse : DefaultResponseDto
    {
        /// <summary>
        /// 位置
        /// </summary>
        public string Postion { get; set; } = string.Empty;

        /// <summary>
        /// 所屬管理處
        /// </summary>
        public string AffiliatedUnit { get; set; } = string.Empty;
    }
}
