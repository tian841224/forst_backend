using admin_backend.DTOs.Documentation;

namespace admin_backend.Interfaces
{
    /// <summary>
    /// 會員註冊使用條款
    /// </summary>
    public interface IDocumentationService
    {
        Task<List<DocumentationResponse>> Get();
        Task<DocumentationResponse> Add(AddDocumentationDto dto);
    }
}
