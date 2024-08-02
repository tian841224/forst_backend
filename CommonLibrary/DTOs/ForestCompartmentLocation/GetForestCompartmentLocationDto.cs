using CommonLibrary.DTOs.Common;

namespace CommonLibrary.DTOs.ForestCompartmentLocation
{
    public class GetForestCompartmentLocationDto: PagedOperationDto
    {
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string? Keyword { get; set; } = string.Empty;
    }
}
