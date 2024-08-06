using admin_backend.DTOs.OperationLog;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface IOperationLogService
    {
        Task<PagedResult<OperationLogResponse>> Get(GetOperationLogDto dto);
        Task Add(AddOperationLogDto dto);
    }
}
