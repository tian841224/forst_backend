using admin_backend.Enums;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.DTOs.Documentation
{
    public class DocumentationResponse : DefaultResponseDto
    {
        /// <summary>
        /// 使用條款類型 1 = 同意書, 2 = 使用說明
        /// </summary>
        public DocumentationEnum Type { get; set; }

        /// <summary>
        /// 使用條款類型 1 = 同意書, 2 = 使用說明
        /// </summary>
        public string TypeName => Type.GetDescription();

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }
}
