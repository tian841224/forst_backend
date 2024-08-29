using admin_backend.DTOs.Case;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    /// <summary>
    /// 案件
    /// </summary>
    public interface ICaseService
    {
        /// <summary>取得單筆案件</summary>
        Task<CaseResponse> Get(int Id);

        /// <summary>取得案件</summary>
        Task<PagedResult<CaseResponse>> Get(GetCaseDto dto);

        /// <summary>新增案件</summary>
        Task<CaseResponse> Add(AddCaseDto dto);

        /// <summary>修改案件</summary>
        Task<CaseResponse> Update(int Id, UpdateCaseDto dto);

        /// <summary>刪除案件</summary>
        Task<CaseResponse> Delete(int Id);
    }
}
