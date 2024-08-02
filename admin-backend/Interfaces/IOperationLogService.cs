using admin_backend.DTOs.OperationLog;

namespace admin_backend.Interfaces
{
    public interface IOperationLogService
    {
        Task<List<OperationLogResponse>> Get(GetOperationLogDto dto);
        Task Add(AddOperationLogDto dto);
    }
}
