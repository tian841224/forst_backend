using CommonLibrary.DTOs.Common;
using CommonLibrary.Enums;

namespace CommonLibrary.DTOs.Documentation
{
    public class DocumentationResponse : DefaultResponseDto
    {
        /// <summary>
        /// 使用條款類型 1 = 同意書, 2 = 使用說明
        /// </summary>
        public DocumentationEnum Type { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }
}
