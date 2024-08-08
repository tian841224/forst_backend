using admin_backend.DTOs.DamageType;
using admin_backend.DTOs.FAQ;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface IFAQService
    {
        /// <summary> 取得常見問題 </summary>
        Task<PagedResult<FAQResponse>> Get(int? Id = null, PagedOperationDto? dto = null);

        /// <summary> 取得常見問題 </summary>
        Task<PagedResult<FAQResponse>> Get(GetFAQDto dto);

        /// <summary> 新增常見問題 </summary>
        Task<FAQResponse> Add(AddFAQDto dto);

        /// <summary> 修改常見問題 </summary>
        Task<FAQResponse> Update(int Id, UpdateFAQDto dto);

        /// <summary> 修改常見問題排序 </summary>
        Task<List<FAQResponse>> UpdateSort(List<SortBasicDto> dto);
    }
}
