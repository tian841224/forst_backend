using admin_backend.DTOs.Documentation;

namespace admin_backend.Interfaces
{
    public interface IDocumentationService
    {
        Task<List<DocumentationResponse>> Get();
        Task<DocumentationResponse> Add(AddDocumentationDto dto);
    }
}
