using admin_backend.DTOs.News;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    public interface INewsService
    {
        /// <summary>取得最新消息 </summary>
        Task<PagedResult<NewsResponse>> Get(int? Id = null, PagedOperationDto? dto = null);
        /// <summary>取得最新消息 </summary>
        Task<PagedResult<NewsResponse>> Get(GetNewsDto dto);
        /// <summary>新增最新消息 </summary>
        Task<NewsResponse> Add(AddNewsDto dto);
        /// <summary>修改最新消息 </summary>
        Task<NewsResponse> Update(int Id, UpdateNewsDto dto);
        /// <summary>刪除最新消息 </summary>
        Task<NewsResponse> Delete(int Id);
    }
}
