using admin_backend.DTOs.CaseHistory;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    /// <summary>
    /// 案件歷程
    /// </summary>
    public interface ICaseHistoryService
    {
        /// <summary>取得單筆案件歷程</summary>
        Task<CaseHistoryResponse> Get(int Id);

        /// <summary>取得案件歷程</summary>
        Task<PagedResult<CaseHistoryResponse>> Get(GetCaseHistoryDto dto);

        /// <summary>新增案件歷程</summary>
        Task Add(AddCaseHistoryDto dto);
    }
}
